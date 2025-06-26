using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// API-controller voor het beheren van expedities en verzendingen.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExpeditionController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public ExpeditionController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expedition>>> GetAll() =>
            await _context.Expeditions.ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Expedition>> Post(Expedition item)
        {
            _context.Expeditions.Add(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                Timestamp = DateTime.UtcNow,
                Activity = "Expeditie toegevoegd",
                Details = $"Zending: {item.ShipmentReference} naar {item.Destination}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = item.Id }, item);
        }
    }

}
