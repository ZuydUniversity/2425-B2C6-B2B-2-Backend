using API.Models;
using API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    /// <summary>
    /// API-controller voor het beheren van planningen.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlanningController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public PlanningController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt alle planningen op.
        /// </summary>
        /// <returns>Alle planningen</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planning>>> GetAll() =>
            await _context.Planning
                .Include(p => p.Order)
                .Include(p => p.ProductionLine)
                .ToListAsync();

        /// <summary>
        /// Plant een order in op een specifieke productielijn.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Gemaakte planning</returns>
        [HttpPost]
        public async Task<ActionResult<Planning>> Post(Planning item)
        {
            _context.Planning.Add(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                Timestamp = DateTime.UtcNow,
                Activity = "Planning toegevoegd",
                Details = $"Planning ID: {item.Id} gepland op {item.PlannedDate}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = item.Id }, item);
        }

        /// <summary>
        /// Wijzigt een bestaande planning.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Planning item)
        {
            if (id != item.Id)
                return BadRequest("ID mismatch");

            if (!OrderExists(item.OrderId) || !ProductionLineExists(item.ProductionLineId))
                return BadRequest("Invalid Order or Production Line ID.");

            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                _context.EventLogs.Add(new EventLog
                {
                    Timestamp = DateTime.UtcNow,
                    Activity = "Planning aangepast",
                    Details = $"Planning ID: {item.Id} aangepast op {item.PlannedDate}"
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanningExists(id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        private bool OrderExists(int id) => _context.Order.Any(e => e.Id == id);
        private bool PlanningExists(int id) => _context.Planning.Any(e => e.Id == id);
        private bool ProductionLineExists(int id) => _context.ProductionLines.Any(e => e.Id == id);

    }
}
