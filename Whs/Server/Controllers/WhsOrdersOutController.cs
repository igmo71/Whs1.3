using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whs.Server.Data;
using Whs.Shared.Models;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhsOrdersOutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WhsOrdersOutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WhsOrderOuts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WhsOrderOut>>> GetWhsOrdersOut()
        {
            return await _context.WhsOrdersOut.ToListAsync();
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

            _context.Entry(whsOrderOut).State = EntityState.Modified;

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
