using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        public WhsOrdersOutController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _settings = configuration.GetSection(WhsOrderSettings.WhsOrder).Get<WhsOrderSettings>();
        }

        // GET: api/WhsOrderOuts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetWhsOrdersOut()
        {
            return await _context.WhsOrdersOut.ToListAsync();
        }

        // GET: api/WhsOrdersOuts/ByQueType
        [HttpGet("DtoByQueType")]
        public ActionResult<WhsOrdersDtoOut> GetWhsOrdersOutDtoByQueType([FromQuery] WhsOrderParameters parameters)
        {
            WhsOrdersDtoOut Dto = new WhsOrdersDtoOut();

            IQueryable<WhsOrderOut> query = _context.WhsOrdersOut
                .Where(e => _settings.MatchingStatusOut.Show.Contains(e.Статус))
                .Search(parameters)
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

            Dto.Items = items.GroupBy(e => e.ТипОчереди).ToDictionary(e => string.IsNullOrEmpty(e.Key) ? "Очередность не указана" : e.Key, e => e.ToArray());

            return Dto;
        }

        // GET: api/WhsOrderOuts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WhsOrderOut>> GetWhsOrderOut(string id)
        {
            var whsOrderOut = await _context.WhsOrdersOut.FindAsync(id);

            if (whsOrderOut == null)
            {
                return NotFound();
            }

            return whsOrderOut;
        }

        // PUT: api/WhsOrderOuts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWhsOrderOut(string id, WhsOrderOut whsOrderOut)
        {
            if (id != whsOrderOut.Документ_Id)
            {
                return BadRequest();
            }

            _context.Update(whsOrderOut);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WhsOrderOutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WhsOrderOuts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WhsOrderOut>> PostWhsOrderOut(WhsOrderOut whsOrderOut)
        {
            _context.WhsOrdersOut.Add(whsOrderOut);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WhsOrderOutExists(whsOrderOut.Документ_Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWhsOrderOut", new { id = whsOrderOut.Документ_Id }, whsOrderOut);
        }

        // DELETE: api/WhsOrderOuts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WhsOrderOut>> DeleteWhsOrderOut(string id)
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

        private bool WhsOrderOutExists(string id)
        {
            return _context.WhsOrdersOut.Any(e => e.Документ_Id == id);
        }
    }
}
