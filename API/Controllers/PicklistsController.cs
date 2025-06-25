using API.Models;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Beheert picklists voor orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PicklistsController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public PicklistsController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt alle picklists op.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picklist>>> GetAll() =>
            await _context.Picklists.ToListAsync();

        /// <summary>
        /// Haalt een specifieke picklist op via ID.
        /// </summary>
        /// <param name="id">Picklist-ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Picklist>> Get(int id)
        {
            var item = await _context.Picklists.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        /// <summary>
        /// Maakt een nieuwe picklist aan.
        /// </summary>
        /// <param name="item">Nieuwe picklist</param>
        [HttpPost]
        public async Task<ActionResult<Picklist>> Post(Picklist item)
        {
            _context.Picklists.Add(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Picklist aangemaakt",
                Details = $"Picklist ID {item.Id} aangemaakt voor Order {item.OrderId}, Product {item.ProductId}, Aantal {item.Quantity}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        /// <summary>
        /// Wijzigt een bestaande picklist.
        /// </summary>
        /// <param name="id">Picklist-ID</param>
        /// <param name="item">Gewijzigde picklist</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Picklist item)
        {
            if (id != item.Id) return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Picklist aangepast",
                Details = $"Picklist ID {item.Id} gewijzigd voor Order {item.OrderId}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verwijdert een picklist op basis van ID.
        /// </summary>
        /// <param name="id">Picklist-ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Picklists.FindAsync(id);
            if (item == null) return NotFound();

            _context.Picklists.Remove(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Picklist verwijderd",
                Details = $"Picklist ID {item.Id} verwijderd voor Order {item.OrderId}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
