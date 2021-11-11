using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Whs.Server.Data;
using Whs.Shared.Models;
using Whs.Shared.Utils;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WhsOrdersOutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WhsOrderSettings _settings;
        private readonly HttpClient _clientHttpService;
        private readonly HttpClient _bitrixClient;
        private readonly ILogger<WhsOrdersOutController> _logger;
        private readonly bool _isNotifySiren;

        public WhsOrdersOutController(ApplicationDbContext context, IConfiguration configuration, IHttpClientFactory clientFactory, ILogger<WhsOrdersOutController> logger)
        {
            _context = context;
            _clientHttpService = clientFactory.CreateClient("ClientHttpService");
            _bitrixClient = clientFactory.CreateClient("SirenBitrixClient");
            _settings = configuration.GetSection(WhsOrderSettings.WhsOrder).Get<WhsOrderSettings>();
            _logger = logger;
            _isNotifySiren = configuration.GetSection("IsNotifySiren").Get<bool>();
        }

        // GET: api/WhsOrdersOut/PrintList
        [HttpGet("PrintList")]
        public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetPrintListAsync([FromQuery] WhsOrderParameters parameters)
        {
            WhsOrderOut[] items = await _context.WhsOrdersOut
                .Where(e => e.Проведен && e.Отгрузить)
                .Search(parameters)
                .OrderBy(e => e.Номер)
                .AsNoTracking()
                .ToArrayAsync();
            return items;
        }

        // GET: api/WhsOrdersOut/DtoByQueType
        [HttpGet("DtoByQueType")]
        public ActionResult<WhsOrdersDtoOut> GetDtoByQueType([FromQuery] WhsOrderParameters parameters)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            WhsOrdersDtoOut dto = new WhsOrdersDtoOut();
            try
            {
                IQueryable<WhsOrderOut> query = _context.WhsOrdersOut
                .Where(e => e.Проведен)
                .Include(e => e.Распоряжения)
                .Include(e => e.Data)
                    .ThenInclude(e => e.ApplicationUser)
                .AsNoTracking();

                List<WhsOrderOut> items;
                if (parameters.SearchBarcode == null)
                {
                    items = query
                        .Where(e => e.Отгрузить)
                        .Search(parameters)
                        .Take(_settings.OrdersPerPage)
                        .ToList();
                }
                else
                {
                    string id = GuidConvert.FromNumStr(parameters.SearchBarcode);
                    
                    items = query
                        .Where(e => e.Документ_Id == id)
                        .ToList();
                    
                    if (items.Count() == 0)
                    {
                        items = query
                            .Where(e => e.Распоряжения
                            .Any(o => o.Распоряжение_Id == id))
                            .ToList();

                        dto.MngrOrderName = items.FirstOrDefault()?.Распоряжения.FirstOrDefault(e => e.Распоряжение_Id == id).Распоряжение_Name;
                    }
                    if (items.Count() == 1)
                        dto.SingleId = items.FirstOrDefault()?.Документ_Id;
                }

                dto.TotalWeight = items.Sum(e => e.Вес).ToString();
                dto.TotalCount = items.Count().ToString();
                dto.Items = items
                    .GroupBy(e => e.ТипОчереди)
                    .ToDictionary(e => string.IsNullOrEmpty(e.Key) ? QueType.Out.NoQue : e.Key, e => e.ToArray());
                dto.Destinations = GetDestinations(parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError($"---> GetDtoByQueType: Exception: {ex};");
                return Problem(ex.Message);
            }

            stopwatch.Stop();
            long duration = stopwatch.ElapsedMilliseconds;
            if (duration > _settings.PerfTime * 1000)
                _logger.LogWarning($"---> GetDtoByQueType: Ok - duration: {duration}ms;");

            return dto;
        }

        private Destination[][] GetDestinations(WhsOrderParameters parameters)
        {
            Destination[] destinationParents = _context.WhsOrdersOut.AsNoTracking()
                .Where(e => e.Проведен == true && e.Склад_Id == parameters.SearchWarehouseId && e.Статус == parameters.SearchStatus && e.НаправлениеДоставкиРодитель_Id != Guid.Empty.ToString())
                .Select(e => new Destination { Id = e.НаправлениеДоставкиРодитель_Id, Name = e.НаправлениеДоставкиРодитель_Name })
                .Distinct().OrderBy(e => e.Name).ToArray();
            Destination[] destinations = _context.WhsOrdersOut.AsNoTracking()
                .Where(e => e.Проведен == true && e.Склад_Id == parameters.SearchWarehouseId && e.Статус == parameters.SearchStatus && e.НаправлениеДоставки_Id != Guid.Empty.ToString())
                .Select(e => new Destination { Id = e.НаправлениеДоставки_Id, Name = e.НаправлениеДоставки_Name })
                .Distinct().OrderBy(e => e.Name).ToArray();
            Destination[][] result = { destinationParents, destinations };
            return result;
        }

        //GET: api/WhsOrdersOut/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WhsOrderOut>> GetAsync(string id)
        {
            WhsOrderOut item = await _context.WhsOrdersOut
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Документ_Id == id);
            if (item == null)
                return NotFound();
            return item;
        }

        // GET: api/WhsOrdersOut/Dto/5
        [HttpGet("Dto/{id}")]
        public async Task<ActionResult<WhsOrderDtoOut>> GetDtoAsync(string id)
        {
            WhsOrderOut item = await _context.WhsOrdersOut
                .Where(e => e.Проведен)
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Документ_Id == id);

            if (item == null)
            {
                _logger.LogError($"---> GetDtoAsync: NotFound; id = {id};");
                return NotFound();
            }

            WhsOrderDtoOut dto = new WhsOrderDtoOut
            {
                Item = item,
                UserName = _context.WhsOrdersDataOut
                    .Include(e => e.ApplicationUser)
                    .Where(e => e.Документ_Id == id)
                    .OrderByDescending(e => e.DateTime)
                    .FirstOrDefault()?.ApplicationUser?.FullName,
                BarcodeBase64 = new NetBarcode.Barcode(GuidConvert.ToNumStr(id), NetBarcode.Type.Code128C, false).GetBase64Image()
            };
            return dto;
        }

        // POST: api/WhsOrdersOut
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WhsOrderOut>> PostAsync(WhsOrderOut whsOrder)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            _context.WhsOrdersOut.Add(whsOrder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (Exists(whsOrder.Документ_Id))
                {
                    _logger.LogError($"---> PostAsync: DbUpdateException Conflict; {whsOrder?.Документ_Name};" +
                        $"{Environment.NewLine}{ex.Message}");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"---> PostAsync: DbUpdateException; {whsOrder?.Документ_Name};" +
                        $"{Environment.NewLine}{ex.Message}");
                    return Problem(ex.Message);
                }
            }

            stopwatch.Stop();
            long duration = stopwatch.ElapsedMilliseconds;
            if (duration > _settings.PerfTime * 1000)
                _logger.LogWarning($"---> PostAsync: Ok - duration>{_settings.PerfTime * 1000}ms: {duration}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder?.ТипОчереди}; Проведен = {whsOrder?.Проведен};");
            else
                _logger.LogInformation($"---> PostAsync: Ok - duration: {duration}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder?.ТипОчереди}; Проведен = {whsOrder?.Проведен};");

            return CreatedAtAction("Get", new { id = whsOrder.Документ_Id }, whsOrder);
        }

        // PUT: api/WhsOrdersOut
        [HttpPut("{id}")]
        [HttpPut("{id}/{barcode}")]
        public async Task<IActionResult> PutAsync(string id, string barcode, WhsOrderOut whsOrder)
        {
            _logger.LogInformation($"---> PutAsync: Start; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder.ТипОчереди}; Проведен = {whsOrder.Проведен};");

            Stopwatch stopwatch = Stopwatch.StartNew();

            if (id != whsOrder.Документ_Id)
            {
                _logger.LogError($"---> PutAsync: BadRequest; {whsOrder?.Документ_Name}; id = {id};");
                return BadRequest();
            }

            if (!(barcode == null || barcode == Guid.Empty.ToString()))  //  Если запрос пришел от клиента
            {
                await CreateProductsDataAsync(whsOrder);

                whsOrder = await PutTo1cAsync(whsOrder);
                if (whsOrder == null)
                {
                    _logger.LogError($"---> PutAsync -> PutTo1cAsync: Problem 1C; id = {id};");
                    return Problem(detail: "Problem 1C");
                }
            }

            IQueryable<ProductOut> productsToRemove = _context.ProductsOut.Where(e => e.Документ_Id == whsOrder.Документ_Id);
            _context.ProductsOut.RemoveRange(productsToRemove);

            IQueryable<MngrOrderOut> mngrOrdersToRemove = _context.MngrOrdersOut.Where(e => e.Документ_Id == whsOrder.Документ_Id);
            _context.MngrOrdersOut.RemoveRange(mngrOrdersToRemove);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"---> PutAsync: Exception (productsToRemove and mngrOrdersToRemove); {whsOrder?.Документ_Name}; id = {id};" +
                        $"{Environment.NewLine}{ex.Message}");
                return Problem(ex.Message);
            }


            _context.Entry(whsOrder).State = EntityState.Modified;

            await _context.ProductsOut.AddRangeAsync(whsOrder.Товары);
            await _context.MngrOrdersOut.AddRangeAsync(whsOrder.Распоряжения);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Exists(id))
                {
                    _logger.LogError($"---> PutAsync: DbUpdateConcurrencyException NotFound; {whsOrder?.Документ_Name}; id = {id};" +
                        $"{Environment.NewLine}{ex.Message}");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"---> PutAsync: DbUpdateConcurrencyException {whsOrder?.Документ_Name}; id = {id};" +
                        $"{Environment.NewLine}{ex.Message}");
                    return Problem(ex.Message);
                }
            }

            await CreateWhsOrderDataAsync(barcode, whsOrder);

            if (_isNotifySiren)
                await NotifySirenAsync(id);

            stopwatch.Stop();
            long duration = stopwatch.ElapsedMilliseconds;
            if (duration > _settings.PerfTime * 1000)
                _logger.LogWarning($"---> PutAsync: Ok - duration>{_settings.PerfTime * 1000}ms: {duration}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder.ТипОчереди}; Проведен = {whsOrder.Проведен};");
            else
                _logger.LogInformation($"---> PutAsync: Ok - duration: {duration}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder.ТипОчереди}; Проведен = {whsOrder.Проведен};");

            return NoContent();
        }

        // GET: api/WhsOrdersOut/Shipment
        [HttpGet("Shipment/{warehouseId}")]
        public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetShipmentAsync(string warehouseId)
        {
            WhsOrderOut[] items = await _context.WhsOrdersOut.Include(e => e.Data).ThenInclude(e => e.ApplicationUser)
                .Where(e => e.Проведен && e.Отгрузить && e.Статус == WhsOrderStatus.Out.ToShipment && e.Склад_Id == warehouseId && !e.ЭтоПеремещение)
                .OrderBy(e => e.Data.Where(e => e.Статус == WhsOrderStatus.Out.ToShipment).OrderByDescending(d => d.DateTime).FirstOrDefault().DateTime)
                .Take(_settings.OrdersPerPage)
                .AsNoTracking().ToArrayAsync();
            return items;
        }

        // PUT: api/WhsOrdersOut/Shipment/5
        [HttpPut("Shipment/{barcode}")]
        public async Task<IActionResult> PutShipmentAsync(string barcode)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            string id = GuidConvert.FromNumStr(barcode);

            WhsOrderOut whsOrder = await _context.WhsOrdersOut
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Документ_Id == id);

            _logger.LogInformation($"---> PutShipmentAsync: Start; {whsOrder?.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder.ТипОчереди}; Проведен = {whsOrder.Проведен};");

            if (whsOrder == null || !(whsOrder.Статус == WhsOrderStatus.Out.ToCollect || whsOrder.Статус == WhsOrderStatus.Out.ToShipment))
            {
                _logger.LogError($"---> PutShipmentAsync: NotFound or Status not match; id = {id}");
                return NotFound();
            }

            whsOrder = await PutTo1cAsync(whsOrder);
            if (whsOrder.Статус == WhsOrderStatus.Out.ToShipment)
                whsOrder = await PutTo1cAsync(whsOrder);
            if (whsOrder == null)
            {
                _logger.LogError($"---> PutShipmentAsync -> PutTo1cAsync: Problem 1C; id = {id}");
                return Problem(detail: "Problem 1C");
            }

            _context.WhsOrdersOut.Update(whsOrder);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Exists(id))
                {
                    _logger.LogError($"---> PutShipmentAsync: DbUpdateConcurrencyException NotFound; {whsOrder.Документ_Name}; id = {id};" +
                        $"{Environment.NewLine}{ex.Message}");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"---> PutShipmentAsync: DbUpdateConcurrencyException; {whsOrder.Документ_Name}; id = {id};" +
                        $"{Environment.NewLine}{ex.Message}");
                    return Problem(ex.Message);
                }
            }

            await CreateWhsOrderDataAsync(null, whsOrder);

            stopwatch.Stop();
            long duration = stopwatch.ElapsedMilliseconds;
            if (duration > _settings.PerfTime * 1000)
                _logger.LogWarning($"---> PutShipmentAsync: Ok - duration>{_settings.PerfTime * 1000}ms: {duration}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder.ТипОчереди}; Проведен = {whsOrder.Проведен};");
            else
                _logger.LogInformation($"---> PutShipmentAsync: Ok - duration: {duration}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder.ТипОчереди}; Проведен = {whsOrder.Проведен};");

            return Ok($"{whsOrder.НомерОчереди}  {whsOrder.Документ_Name}");
        }

        private async Task<WhsOrderOut> PutTo1cAsync(WhsOrderOut whsOrder)
        {
            _logger.LogInformation($"---> PutTo1cAsync: Start; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder?.ТипОчереди}; Проведен = {whsOrder?.Проведен};");
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                string content = JsonSerializer.Serialize(whsOrder);
                StringContent stringContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
                HttpResponseMessage response = await _clientHttpService.PutAsync($"РасходныйОрдерНаТовары/{whsOrder.Документ_Id}", stringContent);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    WhsOrderOut result = JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Результат;

                    stopwatch.Stop();
                    long duration = stopwatch.ElapsedMilliseconds;
                    if (duration > _settings.PerfTime * 1000)
                        _logger.LogWarning($"---> PutTo1cAsync: Ok - duration>{_settings.PerfTime * 1000}ms: {duration}ms; {result.Документ_Name}; Статус = {result.Статус}; ТипОчереди = {result.ТипОчереди}; Проведен = {result.Проведен};");
                    else
                        _logger.LogInformation($"---> PutTo1cAsync: Ok - duration: {duration}ms; {result.Документ_Name}; Статус = {result.Статус}; ТипОчереди = {result.ТипОчереди}; Проведен = {result.Проведен};");
                    
                    return result;
                }
                else
                {
                    _logger.LogError($"---> PutTo1cAsync: Response StatusCode = {response.StatusCode} ({response.ReasonPhrase}); {whsOrder.Документ_Name};" +
                        $"{Environment.NewLine}Ошибка: {JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Ошибка}");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"---> PutTo1cAsync: Exception; {whsOrder.Документ_Name};" +
                    $"{Environment.NewLine}{exception.Message}");
            }

            return null;
        }

        private async Task CreateProductsDataAsync(WhsOrderOut whsOrder)
        {
            List<ProductDataOut> productsData = new List<ProductDataOut>();
            foreach (ProductOut product in whsOrder.Товары)
                if (product.КоличествоПлан != product.КоличествоФакт)
                    productsData.Add(new ProductDataOut(product));
            await _context.ProductsDataOut.AddRangeAsync(productsData);
            await _context.SaveChangesAsync();
        }

        private async Task CreateWhsOrderDataAsync(string barcode, WhsOrderOut whsOrder)
        {
            WhsOrderDataOut whsOrderData = new WhsOrderDataOut
            {
                DateTime = DateTime.Now,
                Статус = whsOrder.Статус,
                Документ_Id = whsOrder.Документ_Id,
                ApplicationUserId = barcode == null ? null : GuidConvert.FromNumStr(barcode)
            };
            await _context.WhsOrdersDataOut.AddAsync(whsOrderData);
            await _context.SaveChangesAsync();
        }

        private async Task NotifySirenAsync(string id)
        {
            try
            {
                WhsOrderOut order = _context.WhsOrdersOut.Find(id);
                if (order != null)
                {
                    //_logger.LogInformation($"---> NotifySirenAsync: Order = {order.Номер} {order.Дата}; ТипОчереди = {order.ТипОчереди}; Статус = {order.Статус}; Отгрузить = {order.Отгрузить}; Проведен = {order.Проведен};");
                    if (order.Проведен && order.ТипОчереди == QueType.Out.LiveQue && order.Статус == WhsOrderStatus.Out.Prepared && order.Отгрузить)
                    {
                        //_logger.LogInformation($"---> NotifySirenAsync: Siren => Squeak; Склад = {order.Склад_Name};");
                        _ = await _bitrixClient.GetAsync($"?type=siren&params=0&sklad={order.Склад_Name}");
                    }

                    int ordersCount = _context.WhsOrdersOut
                        .Where(e => e.Склад_Name == order.Склад_Name && e.Проведен && e.ТипОчереди == QueType.Out.LiveQue && e.Статус == WhsOrderStatus.Out.Prepared && e.Отгрузить)
                        .Count();
                    //_logger.LogInformation($"---> NotifySirenAsync: ordersCount = {ordersCount};");
                    if (ordersCount == 0)
                    {
                        //_logger.LogInformation($"---> NotifySirenAsync: Lamp => SwitchOff; Склад = {order.Склад_Name};");
                        _ = await _bitrixClient.GetAsync($"?type=lamp&params=1&sklad={order.Склад_Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"---> NotifySirenAsync: Exception Message = {ex.Message}");
                return;
            }
        }

        private bool Exists(string id) => _context.WhsOrdersOut.Any(e => e.Документ_Id == id);
    }
}
