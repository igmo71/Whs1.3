using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whs.Server.Data;
using Whs.Shared.Models;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditingCausesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EditingCausesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EditingCauses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EditingCause>>> GetEditingCauses()
        {
            return await _context.EditingCauses.ToListAsync();
        }

        // GET: api/EditingCauses/ForIn
        [HttpGet("ForIn")]
        public async Task<ActionResult<IEnumerable<EditingCause>>> GetEditingCausesForIn()
        {
            return await _context.EditingCausesIn.AsNoTracking().ToListAsync();
        }

        // GET: api/EditingCauses/ForOut
        [HttpGet("ForOut")]
        public async Task<ActionResult<IEnumerable<EditingCause>>> GetEditingCausesForOut()
        {
            return await _context.EditingCausesOut.AsNoTracking().ToListAsync();
        }


        // GET: api/EditingCauses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EditingCause>> GetEditingCause(Guid id)
        {
            var editingCause = await _context.EditingCauses.FindAsync(id);

            if (editingCause == null)
            {
                return NotFound();
            }

            return editingCause;
        }

        // PUT: api/EditingCauses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditingCause(Guid id, EditingCause editingCause)
        {
            if (id != editingCause.Id)
            {
                return BadRequest();
            }

            _context.Entry(editingCause).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditingCauseExists(id))
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

        // POST: api/EditingCauses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EditingCause>> PostEditingCause(EditingCause editingCause)
        {
            _context.EditingCauses.Add(editingCause);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEditingCause", new { id = editingCause.Id }, editingCause);
        }

        // DELETE: api/EditingCauses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EditingCause>> DeleteEditingCause(Guid id)
        {
            var editingCause = await _context.EditingCauses.FindAsync(id);
            if (editingCause == null)
            {
                return NotFound();
            }

            _context.EditingCauses.Remove(editingCause);
            await _context.SaveChangesAsync();

            return editingCause;
        }

        private bool EditingCauseExists(Guid id)
        {
            return _context.EditingCauses.Any(e => e.Id == id);
        }
    }
}
