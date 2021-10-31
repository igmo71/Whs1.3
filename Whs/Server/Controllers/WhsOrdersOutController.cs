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
        public async Task<ActionResult<WhsOrdersDtoOut>> GetDtoByQueTypeAsync([FromQuery] WhsOrderParameters parameters)
        {
            WhsOrdersDtoOut dto = new WhsOrdersDtoOut();
            IQueryable<WhsOrderOut> query = _context.WhsOrdersOut
                .Where(e => e.Проведен)
                .Include(e => e.Распоряжения)
                .Include(e => e.Data)
                    .ThenInclude(e => e.ApplicationUser)
                .AsNoTracking();

            IEnumerable<WhsOrderOut> items;
            if (parameters.SearchBarcode == null)
            {
                query = query.Where(e => e.Отгрузить);
                query = query.Search(parameters);
                dto.TotalWeight = query.Sum(e => e.Вес).ToString();
                dto.TotalCount = query.Count().ToString();
                items = query.Take(_settings.OrdersPerPage).AsEnumerable();
            }
            else
            {
                string id = GuidConvert.FromNumStr(parameters.SearchBarcode);
                items = query.Where(e => e.Документ_Id == id).AsEnumerable();
                if (items.Count() == 0)
                {
                    items = query.Where(e => e.Распоряжения.Any(o => o.Распоряжение_Id == id)).AsEnumerable();
                    dto.MngrOrderName = items.FirstOrDefault()?.Распоряжения.FirstOrDefault(e => e.Распоряжение_Id == id).Распоряжение_Name;
                }
                dto.TotalCount = query.Count().ToString();
                dto.TotalWeight = query.Sum(e => e.Вес).ToString();
                if (items.Count() == 1)
                    dto.SingleId = items.FirstOrDefault()?.Документ_Id;
            }
            dto.Items = items
                .GroupBy(e => e.ТипОчереди)
                .ToDictionary(e => string.IsNullOrEmpty(e.Key) ? QueType.Out.NoQue : e.Key, e => e.ToArray());
            dto.Destinations = await GetDestinationsAsync(parameters);

            return dto;
        }

        private async Task<Destination[][]> GetDestinationsAsync(WhsOrderParameters parameters)
        {
            Destination[] destinationParents = await _context.WhsOrdersOut.AsNoTracking()
                .Where(e => e.Проведен == true && e.Склад_Id == parameters.SearchWarehouseId && e.Статус == parameters.SearchStatus && e.НаправлениеДоставкиРодитель_Id != Guid.Empty.ToString())
                .Select(e => new Destination { Id = e.НаправлениеДоставкиРодитель_Id, Name = e.НаправлениеДоставкиРодитель_Name })
                .Distinct().OrderBy(e => e.Name).ToArrayAsync();
            Destination[] destinations = await _context.WhsOrdersOut.AsNoTracking()
                .Where(e => e.Проведен == true && e.Склад_Id == parameters.SearchWarehouseId && e.Статус == parameters.SearchStatus && e.НаправлениеДоставки_Id != Guid.Empty.ToString())
                .Select(e => new Destination { Id = e.НаправлениеДоставки_Id, Name = e.НаправлениеДоставки_Name })
                .Distinct().OrderBy(e => e.Name).ToArrayAsync();
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
                    throw;
                }
            }

            stopwatch.Stop();

            _logger.LogInformation($"---> PostAsync: Ok - duration: {stopwatch.ElapsedMilliseconds}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder?.ТипОчереди}; Проведен = {whsOrder?.Проведен};");
            
            return CreatedAtAction("Get", new { id = whsOrder.Документ_Id }, whsOrder);
        }

        // PUT: api/WhsOrdersOut
        [HttpPut("{id}")]
        [HttpPut("{id}/{barcode}")]
        public async Task<IActionResult> PutAsync(string id, string barcode, WhsOrderOut whsOrder)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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
                    _logger.LogError($"---> PutAsync: Problem 1C; id = {id};");
                    return Problem(detail: "Problem 1C");
                }
            }

            IQueryable<ProductOut> productsToRemove = _context.ProductsOut.Where(e => e.Документ_Id == whsOrder.Документ_Id);
            _context.ProductsOut.RemoveRange(productsToRemove);
            IQueryable<MngrOrderOut> mngrOrdersToRemove = _context.MngrOrdersOut.Where(e => e.Документ_Id == whsOrder.Документ_Id);
            _context.MngrOrdersOut.RemoveRange(mngrOrdersToRemove);
            await _context.SaveChangesAsync();

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
                    throw;
                }
            }

            await CreateWhsOrderDataAsync(barcode, whsOrder);

            stopwatch.Stop();

            _logger.LogInformation($"---> PutAsync: Ok - duration: {stopwatch.ElapsedMilliseconds}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder.ТипОчереди}; Проведен = {whsOrder.Проведен};");

            if (_isNotifySiren)
                await NotifySirenAsync(id);

            return NoContent();
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

        private async Task<WhsOrderOut> PutTo1cAsync(WhsOrderOut whsOrder)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
         
            try
            {
                string content = JsonSerializer.Serialize(whsOrder);
                StringContent stringContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
                HttpResponseMessage response = await _clientHttpService.PutAsync($"РасходныйОрдерНаТовары/{whsOrder.Документ_Id}", stringContent);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Результат;
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

            stopwatch.Stop();

            _logger.LogInformation($"---> PutTo1cAsync: Ok - duration: {stopwatch.ElapsedMilliseconds}ms; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус}; ТипОчереди = {whsOrder?.ТипОчереди}; Проведен = {whsOrder?.Проведен};");

            return null;
        }

        private async Task NotifySirenAsync(string id)
        {
            try
            {
                WhsOrderOut order = _context.WhsOrdersOut.Find(id);
                if (order != null)
                {
                    _logger.LogInformation($"---> NotifySirenAsync: Order = {order.Номер} {order.Дата}; ТипОчереди = {order.ТипОчереди}; Статус = {order.Статус}; Отгрузить = {order.Отгрузить}; Проведен = {order.Проведен};");
                    if (order.Проведен && order.ТипОчереди == QueType.Out.LiveQue && order.Статус == WhsOrderStatus.Out.Prepared && order.Отгрузить)
                    {
                        _logger.LogInformation($"---> NotifySirenAsync: Siren => Squeak; Склад = {order.Склад_Name};");
                        _ = await _bitrixClient.GetAsync($"?type=siren&params=0&sklad={order.Склад_Name}");
                    }

                    int ordersCount = _context.WhsOrdersOut
                        .Where(e => e.Склад_Name == order.Склад_Name && e.Проведен && e.ТипОчереди == QueType.Out.LiveQue && e.Статус == WhsOrderStatus.Out.Prepared && e.Отгрузить)
                        .Count();
                    _logger.LogInformation($"---> NotifySirenAsync: ordersCount = {ordersCount};");
                    if (ordersCount == 0)
                    {
                        _logger.LogInformation($"---> NotifySirenAsync: Lamp => SwitchOff; Склад = {order.Склад_Name};");
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
            string id = GuidConvert.FromNumStr(barcode);
            WhsOrderOut whsOrder = await _context.WhsOrdersOut
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Документ_Id == id);

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
                _logger.LogError($"---> PutShipmentAsync: Problem 1C; id = {id}");
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
                    throw;
                }
            }

            await CreateWhsOrderDataAsync(null, whsOrder);
            _logger.LogInformation($"---> PutShipmentAsync: Ok; {whsOrder.Документ_Name}; Статус = {whsOrder.Статус};");
            return Ok($"{whsOrder.НомерОчереди}  {whsOrder.Документ_Name}");
        }

        // GET: api/WhsOrdersOut/
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetListAsync()
        //{
        //    return await _context.WhsOrdersOut
        //        .Include(e => e.Товары)
        //        .Include(e => e.Распоряжения)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}

        // DELETE: api/WhsOrdersOut/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WhsOrderOut>> DeleteAsync(string id)
        {
            WhsOrderOut item = await _context.WhsOrdersOut.FindAsync(id);
            if (item == null)
                return NotFound();
            _context.WhsOrdersOut.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
