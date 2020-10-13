using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Generic;
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
    public class WhsOrdersOutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WhsOrderSettings _settings;
        private readonly HttpClient _clientHttpService;
        private readonly ILogger<WhsOrdersOutController> _logger;

        public WhsOrdersOutController(ApplicationDbContext context, IConfiguration configuration, IHttpClientFactory clientFactory, ILogger<WhsOrdersOutController> logger)
        {
            _context = context;
            _clientHttpService = clientFactory.CreateClient("ClientHttpService");
            _settings = configuration.GetSection(WhsOrderSettings.WhsOrder).Get<WhsOrderSettings>();
            _logger = logger;
        }

        // GET: api/WhsOrdersOut/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetListAsync()
        {
            return await _context.WhsOrdersOut
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .Take(_settings.OrdersPerPage)
                .ToListAsync();
        }

        // GET: api/WhsOrdersOut/DtoByQueType
        [HttpGet("DtoByQueType")]
        public ActionResult<WhsOrdersDtoOut> GetDtoByQueType([FromQuery] WhsOrderParameters parameters)
        {
            //_logger.LogInformation($"---> GetDtoByQueType: Begin");
            WhsOrdersDtoOut dto = new WhsOrdersDtoOut();

            IQueryable<WhsOrderOut> query = _context.WhsOrdersOut
                .Where(e => e.Проведен)
                .Include(e => e.Распоряжения)
                .Include(e => e.Data)
                    .ThenInclude(e => e.ApplicationUser)
                .OrderByDescending(e => e.ВесовойКоэффициент)
                .ThenBy(e => e.СрокВыполнения)
                .AsNoTracking();

            IEnumerable<WhsOrderOut> items;
            if (parameters.SearchBarcode == null)
            {
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
                if (items.Count() == 1)
                    dto.SingleId = items.FirstOrDefault()?.Документ_Id;
            }
            dto.Items = items
                .GroupBy(e => e.ТипОчереди)
                .ToDictionary(e => string.IsNullOrEmpty(e.Key) ? "Очередность не указана" : e.Key, e => e.ToArray());
            //_logger.LogInformation($"---> GetDtoByQueType: Ok {dto.Items.Count}");
            return dto;
        }

        // GET: api/WhsOrdersOut/PrintList
        [HttpGet("PrintList")]
        public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetPrintListAsync([FromQuery] WhsOrderParameters parameters)
        {
            WhsOrderOut[] items = await _context.WhsOrdersOut
                .Search(parameters)
                .Where(e => e.Проведен)
                .OrderByDescending(e => e.Номер)
                .AsNoTracking().ToArrayAsync();
            return items;
        }

        // GET: api/WhsOrdersOut/5
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
            //_logger.LogInformation($"---> GetDtoAsync/{id}: Begin");
            WhsOrderOut item = await _context.WhsOrdersOut
                .Where(e => e.Проведен)
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Документ_Id == id);

            if (item == null)
            {
                _logger.LogError($"---> GetDto/{id}: NotFound");
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
            //_logger.LogInformation($"---> GetDtoAsync/{id}: Ok {dto.Item.Документ_Name}");
            return dto;
        }

        // POST: api/WhsOrdersOut
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WhsOrderOut>> PostAsync(WhsOrderOut whsOrder)
        {
            //_logger.LogInformation($"---> PostAsync: Begin {whsOrder.Номер} - {whsOrder.Дата} - {whsOrder.НомерОчереди} - {whsOrder.Статус} - " +
            //    $"{whsOrder.ТипОчереди} - {whsOrder.СрокВыполнения} - {whsOrder.Комментарий} ");
            _context.WhsOrdersOut.Add(whsOrder);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (Exists(whsOrder.Документ_Id))
                {
                    _logger.LogError($"---> PostAsync: DbUpdateException Conflict {whsOrder.Документ_Name}{Environment.NewLine}{ex.Message}");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"---> PostAsync: DbUpdateException {whsOrder.Документ_Name}{Environment.NewLine}{ex.Message}");
                    throw;
                }
            }
            _logger.LogInformation($"---> PostAsync: Ok {whsOrder.Документ_Name}");
            return CreatedAtAction("Get", new { id = whsOrder.Документ_Id }, whsOrder);
        }

        // PUT: api/WhsOrdersOut
        [HttpPut("{id}")]
        [HttpPut("{id}/{barcode}")]
        public async Task<IActionResult> PutAsync(string id, string barcode, WhsOrderOut whsOrder)
        {
            //_logger.LogInformation($"---> PutUAsync/{id}: Begin {whsOrder.Номер} - {whsOrder.Дата} - {whsOrder.НомерОчереди} - {whsOrder.Статус} - " +
            //    $"{whsOrder.ТипОчереди} - {whsOrder.СрокВыполнения} - {whsOrder.Комментарий} ");

            if (id != whsOrder.Документ_Id)
            {
                _logger.LogError($"---> PutUAsync/{id}: BadRequest {whsOrder.Документ_Name}");
                return BadRequest();
            }

            if (!(barcode == null || barcode == Guid.Empty.ToString()))  //  Если запрос пришел от клиента
            {
                await CreateProductsDataAsync(whsOrder);

                whsOrder = await PutTo1cAsync(whsOrder);
                if (whsOrder == null)
                {
                    _logger.LogError($"---> PutAsync/{id}: Problem - 1C");
                    return Problem(detail: "Problem - 1C");
                }
            }

            await CreateWhsOrderDataAsync(barcode, whsOrder);

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
                    _logger.LogError($"---> PutAsync/{id}: DbUpdateConcurrencyException NotFound {whsOrder.Документ_Name}{Environment.NewLine}{ex.Message}");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"---> PutAsync/{id}: DbUpdateConcurrencyException {whsOrder.Документ_Name}{Environment.NewLine}{ex.Message}");
                    throw;
                }
            }
            _logger.LogInformation($"---> PutAsync: Ok {whsOrder.Документ_Name}");
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
            //_logger.LogInformation($"---> PutTo1cAsync: Begin {whsOrder.Документ_Name}");
            string responseContent = string.Empty;
            try
            {
                string content = JsonSerializer.Serialize(whsOrder);
                StringContent stringContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
                HttpResponseMessage response = await _clientHttpService.PutAsync($"РасходныйОрдерНаТовары/{whsOrder.Документ_Id}", stringContent);
                responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"---> PutTo1cAsync: Ok {whsOrder.Документ_Name}");
                    return JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Результат;
                }
                else
                {
                    _logger.LogError($"---> PutTo1cAsync: {whsOrder.Документ_Name}{Environment.NewLine}" +
                        $"Ошибка: {JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Ошибка}");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"---> PutTo1cAsync: {whsOrder.Документ_Name}{Environment.NewLine}{exception.Message}{Environment.NewLine}{responseContent} ");
            }
            _logger.LogWarning($"---> PutTo1cAsync: NULL {whsOrder.Документ_Name}");
            return null;
        }

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

        private bool Exists(string id) => _context.WhsOrdersOut.Any(e => e.Документ_Id == id);
    }
}
