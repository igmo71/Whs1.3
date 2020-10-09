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

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WhsOrderSettings _settings;

        public WarehousesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _settings = configuration.GetSection(WhsOrderSettings.WhsOrder).Get<WhsOrderSettings>();
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            return await _context.Warehouses.ToListAsync();
        }

        // GET: api/Warehouses/ForIn
        [HttpGet("ForIn")]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehousesForIn()
        {
            Warehouse[] items = await _context.WhsOrdersIn.AsNoTracking()
               .Where(e => e.Статус == "Подготовлено")
               .Select(e => new Warehouse { Id = e.Склад_Id, Name = e.Склад_Name })
               .Distinct().ToArrayAsync();
            if (items == null)
                return NoContent();
            return items;
        }

        // GET: api/Warehouses/ForOut
        [HttpGet("ForOut")]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehousesForOut()
        {
            Warehouse[] items = await _context.WhsOrdersOut.AsNoTracking()
               //.Where(e => e.Статус == "Подготовлено")
               .Select(e => new Warehouse { Id = e.Склад_Id, Name = e.Склад_Name })
               .Distinct().ToArrayAsync();
            if (items == null)
                return NoContent();
            return items;
        }

        // GET: api/Warehouses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(string id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return warehouse;
        }

        // PUT: api/Warehouses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(string id, Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return BadRequest();
            }

            _context.Entry(warehouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseExists(id))
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

        // POST: api/Warehouses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Warehouse>> PostWarehouse(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WarehouseExists(warehouse.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWarehouse", new { id = warehouse.Id }, warehouse);
        }

        // DELETE: api/Warehouses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Warehouse>> DeleteWarehouse(string id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();

            return warehouse;
        }

        private bool WarehouseExists(string id)
        {
            return _context.Warehouses.Any(e => e.Id == id);
        }
    }
}
