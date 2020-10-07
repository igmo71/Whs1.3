﻿using System;
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
        public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetWhsOrdersOut()
        {
            return await _context.WhsOrdersOut
                .Include(e => e.Товары)
                .Include(e => e.Распоряжения)
                .AsNoTracking()
                .Take(20)
                .ToListAsync();
        }

        // GET: api/WhsOrdersOuts/DtoByQueType
        [HttpGet("DtoByQueType")]
        public ActionResult<WhsOrdersDtoOut> GetDtoByQueType([FromQuery] WhsOrderParameters parameters)
        {
            WhsOrdersDtoOut Dto = new WhsOrdersDtoOut();

            IQueryable<WhsOrderOut> query = _context.WhsOrdersOut
                .Where(e => e.Проведен)
                .Where(e => _settings.MatchingStatusOut.Show.Contains(e.Статус))
                .Search(parameters)
                .Take(20)
                .Include(e => e.Распоряжения)
                .OrderByDescending(e => e.ВесовойКоэффициент)
                .ThenBy(e => e.СрокВыполнения)
                .AsNoTracking();

            IEnumerable<WhsOrderOut> items;
            if (parameters.SearchBarcode == null)
            {
                items = query.AsEnumerable();
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
        public async Task<ActionResult<WhsOrderOut>> Get(string id)
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
        public async Task<ActionResult<WhsOrderDtoOut>> GetDto(string id)
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
                    _logger.LogError($"---> PostAsync: Conflict {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"---> PostAsync: {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}");
                    throw;
                }
            }

            return CreatedAtAction("Get", new { id = whsOrderOut.Документ_Id }, whsOrderOut);
        }

        // PUT: api/WhsOrdersOut/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAsync(string id, WhsOrderOut whsOrderOut)
        //{
        //    if (id != whsOrderOut.Документ_Id)
        //    {
        //        _logger.LogError($"---> PutAsync/{id}: BadRequest" +
        //            $"{Environment.NewLine}{whsOrderOut.Номер}");
        //        return BadRequest();
        //    }

        //    _context.Update(whsOrderOut);

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Exists(id))
        //        {
        //            _logger.LogError($"---> PutAsync/{id}: DbUpdateConcurrencyException - NotFound" +
        //                $"{Environment.NewLine}{whsOrderOut.Документ_Name}");
        //            return NotFound();
        //        }
        //        else
        //        {
        //            _logger.LogError($"---> PutAsync/{id}: DbUpdateConcurrencyException" +
        //                $"{Environment.NewLine}{whsOrderOut.Документ_Name}");
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // PUT: api/WhsOrdersOut/UpdateStatus/5
        //[HttpPut("UpdateStatus/{id}")]
        //public async Task<ActionResult<WhsOrderOut>> PutUpdateStatusAsync(string id, WhsOrderOut whsOrderOut)
        [HttpPut("{id}")]
        [HttpPut("{id}/{to1C}")]
        public async Task<IActionResult> PutAsync(string id, string to1C, WhsOrderOut whsOrderOut)
        {
            if (id != whsOrderOut.Документ_Id)
            {
                _logger.LogError($"---> PutUpdateStatusAsync/{id}: BadRequest {whsOrderOut.Номер}");
                return BadRequest();
            }

            if (to1C == "to1C")
            {
                whsOrderOut = await PutTo1cAsync(whsOrderOut);
                if (whsOrderOut == null)
                {
                    _logger.LogError($"---> PutUpdateStatusAsync/{id}: Problem - 1C");
                    return Problem();
                }
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
                    _logger.LogError($"---> PutUpdateStatusAsync/{id}: NotFound {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"---> PutUpdateStatusAsync/{id}: {whsOrderOut.Документ_Name}{Environment.NewLine}{ex.Message}");
                    throw;
                }
            }

            //return whsOrderOut;
            return NoContent();
        }

        private async Task<WhsOrderOut> PutTo1cAsync(WhsOrderOut whsOrderOut)
        {
            string content = JsonSerializer.Serialize(whsOrderOut);
            StringContent stringContent = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
            HttpResponseMessage response = await _clientHttpService.PutAsync($"РасходныйОрдерНаТовары/{whsOrderOut.Документ_Id}", stringContent);
            string responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                Response1cOut response1C = JsonSerializer.Deserialize<Response1cOut>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"---> PutTo1cAsync: {whsOrderOut.Документ_Name} - Ok");
                    return response1C.Результат;
                }
                else
                {
                    _logger.LogError($"---> PutTo1cAsync: {whsOrderOut.Документ_Name}{Environment.NewLine}Ошибка: {response1C.Ошибка}");
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
