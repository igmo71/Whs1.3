using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class DestinationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly WhsOrderSettings _settings;

        public DestinationsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _settings = configuration.GetSection(WhsOrderSettings.WhsOrder).Get<WhsOrderSettings>();
        }

        // GET: api/Destinations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Destination>>> GetDestination()
        {
            Destination[] items = await _context.WhsOrdersOut.AsNoTracking()
                .Where(e => _settings.MatchingStatusOut.Show.Contains(e.Статус))
                .Select(e => new Destination { Id = e.НаправлениеДоставки_Id, Name = e.НаправлениеДоставки_Name })
                .Distinct().ToArrayAsync();
            if (items == null || items.Count() == 0)
                return NoContent();
            items.FirstOrDefault(e => e.Id == Guid.Empty.ToString()).Name = "- Без направления -";
            Destination[] item = { new Destination { Id = "0", Name = "- Все направления -" } };
            Destination[] result = new Destination[items.Length + 1];
            Array.Copy(item, result, 1);
            Array.Copy(items, 0, result, 1, items.Length);
            return result;
        }
    }
}
