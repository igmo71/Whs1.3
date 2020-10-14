using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whs.Server.Data;
using Whs.Shared.Models;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WarehousesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses() => await _context.Warehouses.AsNoTracking().ToListAsync();

        // GET: api/Warehouses/ForIn
        [HttpGet("ForIn")]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehousesForIn()
        {
            Warehouse[] items = await _context.WhsOrdersIn.AsNoTracking()
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
               .Select(e => new Warehouse { Id = e.Склад_Id, Name = e.Склад_Name })
               .Distinct().ToArrayAsync();
            if (items == null)
                return NoContent();
            return items;
        }
    }
}
