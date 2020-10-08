using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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


        // GET: api/WhsOrdersOuts/
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

        // GET: api/WhsOrdersOuts/DtoByQueType
        [HttpGet("DtoByQueType")]
        public ActionResult<WhsOrdersDtoOut> GetDtoByQueType([FromQuery] WhsOrderParameters parameters)
        {
            WhsOrdersDtoOut Dto = new WhsOrdersDtoOut();

            IQueryable<WhsOrderOut> query = _context.WhsOrdersOut
                .Where(e => e.Проведен)
                //.Where(e => _settings.MatchingStatusOut.Show.Contains(e.Статус))
                .Search(parameters)
                .Take(_settings.OrdersPerPage)
                .Include(e => e.Распоряжения)
                .OrderByDescending(e => e.ВесовойКоэффициент)
                .ThenBy(e => e.СрокВыполнения)
                .AsNoTracking();

            IEnumerable<WhsOrderOut> items;
            if (parameters.SearchBarcode == null)
            {
                items = query
                    .Where(e => e.Статус == _settings.MatchingStatusOut.New)
                    .AsEnumerable();
            }
            else
            {
                string id = GuidConvert.FromNumStr(parameters.SearchBarcode);
                items = query.Where(e => e.Документ_Id == id).AsEnumerable();
                if (items.Count() == 0)   //  Если не найдено, ищем по штрихкоду распоряжения
                {
                    items = query.Where(e => e.Распоряжения.Any(o => o.Распоряжение_Id == id)).AsEnumerable();
                    Dto.MngrOrderName = items.FirstOrDefault()?.Распоряжения.FirstOrDefault(e => e.Распоряжение_Id == id).Распоряжение_Name;
                }
                if (items.Count() == 1)
                    Dto.SingleId = items.FirstOrDefault()?.Документ_Id;
            }
            Dto.TotalWeight = items.Sum(e => e.Вес).ToString();
            Dto.Items = items.GroupBy(e => e.ТипОчереди).ToDictionary(e => string.IsNullOrEmpty(e.Key) ? "Очередность не указана" : e.Key, e => e.ToArray());
            return Dto;
        }

        // GET: api/WhsOrdersOut/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WhsOrderOut>> GetAsync(string id)
        {
            var whsOrderOut = await _context.WhsOrdersOut
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Документ_Id == id);

            if (whsOrderOut == null)
            {
                return NotFound();
            }

            return whsOrderOut;
        }

        // GET: api/WhsOrdersOut/Dto/5
        [HttpGet("Dto/{id}")]
        public async Task<ActionResult<WhsOrderDtoOut>> GetDtoAsync(string id)
        {
            WhsOrderDtoOut Dto = new WhsOrderDtoOut
            {
                Item = await _context.WhsOrdersOut
                .Where(e => e.Проведен)
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Документ_Id == id),
                BarcodeBase64 = new NetBarcode.Barcode(GuidConvert.ToNumStr(id), NetBarcode.Type.Code128C, false).GetBase64Image()
            };

            Dto.IsAutoPrint = Dto.Item.Статус == _settings.MatchingStatusOut.New;

            if (Dto.Item == null)
            {
                _logger.LogError($"---> GetDto/{id}: NotFound");
                return NotFound();
            }

            return Dto;
        }

        // POST: api/WhsOrdersOut
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WhsOrderOut>> PostAsync(WhsOrderOut whsOrderOut)
        {
            _context.WhsOrdersOut.Add(whsOrderOut);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (Exists(whsOrderOut.Документ_Id))
                {
                    _logger.LogError($"---> PostAsync: Conflict {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"---> PostAsync: {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    throw;
                }
            }

            return CreatedAtAction("GetAsync", new { id = whsOrderOut.Документ_Id }, whsOrderOut);
        }

        // POST: api/WhsOrdersOut
        [HttpPut("{id}")]
        [HttpPut("{id}/{barcode}")]
        public async Task<IActionResult> PutAsync(string id, string barcode, WhsOrderOut whsOrderOut)
        {
            if (id != whsOrderOut.Документ_Id)
            {
                _logger.LogError($"---> PutUpdateStatusAsync/{id}: BadRequest {whsOrderOut.Номер}");
                return BadRequest();
            }

            if (!(barcode == null || barcode == Guid.Empty.ToString()))
            {
                whsOrderOut = await PutTo1cAsync(whsOrderOut);
                if (whsOrderOut == null)
                {
                    _logger.LogError($"---> PutAsync/{id}: Problem - 1C");
                    return Problem(detail: "Problem - 1C");
                }

                //WhsOrderDataOut whsOrderDataOut = new WhsOrderDataOut
                //{
                //    WhsOrderOutId = whsOrderOut.Документ_Id,
                //    ApplicationUserId = GuidConvert.FromNumStr(barcode),
                //    OldStatus = oldStatus,
                //}
            }

            IQueryable<ProductOut> productsToRemove = _context.ProductsOut.Where(e => e.Документ_Id == whsOrderOut.Документ_Id);
            _context.ProductsOut.RemoveRange(productsToRemove);
            IQueryable<MngrOrderOut> mngrOrdersOutToRemove = _context.MngrOrdersOut.Where(e => e.Документ_Id == whsOrderOut.Документ_Id);
            _context.MngrOrdersOut.RemoveRange(mngrOrdersOutToRemove);
            await _context.SaveChangesAsync();

            _context.Entry(whsOrderOut).State = EntityState.Modified;
            await _context.ProductsOut.AddRangeAsync(whsOrderOut.Товары);
            await _context.MngrOrdersOut.AddRangeAsync(whsOrderOut.Распоряжения);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!Exists(id))
                {
                    _logger.LogError($"---> PutAsync/{id}: NotFound {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"---> PutAsync/{id}: {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    throw;
                }
            }

            return NoContent();
        }

        private async Task<WhsOrderOut> PutTo1cAsync(WhsOrderOut whsOrderOut)
        {
            string responseContent = string.Empty;
            try
            {
                string content = JsonSerializer.Serialize(whsOrderOut);
                StringContent stringContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
                HttpResponseMessage response = await _clientHttpService.PutAsync($"РасходныйОрдерНаТовары/{whsOrderOut.Документ_Id}", stringContent);
                responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"---> PutTo1cAsync: {whsOrderOut.Документ_Name} - Ok");
                    return JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Результат;
                }
                else
                {
                    _logger.LogError($"---> PutTo1cAsync: {whsOrderOut.Документ_Name}{Environment.NewLine}" +
                        $"Ошибка: {JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Ошибка}");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"---> PutTo1cAsync: {whsOrderOut.Документ_Name}{Environment.NewLine}{exception.Message}{Environment.NewLine}{responseContent} ");
            }
            return null;
        }

        // DELETE: api/WhsOrdersOut/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WhsOrderOut>> DeleteAsync(string id)
        {
            var whsOrderOut = await _context.WhsOrdersOut.FindAsync(id);
            if (whsOrderOut == null)
            {
                return NotFound();
            }

            _context.WhsOrdersOut.Remove(whsOrderOut);
            await _context.SaveChangesAsync();

            return whsOrderOut;
        }

        private bool Exists(string id) => _context.WhsOrdersOut.Any(e => e.Документ_Id == id);
    }
}
