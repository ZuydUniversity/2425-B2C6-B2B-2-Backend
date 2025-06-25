using API.Models;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Beheert afkeuringsformulieren voor bestellingen.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RejectionFormsController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public RejectionFormsController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt alle afkeuringsformulieren op.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RejectionForm>>> GetAll() =>
            await _context.RejectionForms.ToListAsync();

        /// <summary>
        /// Haalt een specifiek afkeuringsformulier op.
        /// </summary>
        /// <param name="id">ID van het formulier</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<RejectionForm>> Get(int id)
        {
            var item = await _context.RejectionForms.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        /// <summary>
        /// Maakt een nieuw afkeuringsformulier aan.
        /// </summary>
        /// <param name="item">Formuliergegevens</param>
        [HttpPost]
        public async Task<ActionResult<RejectionForm>> Post(RejectionForm item)
        {
            _context.RejectionForms.Add(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Order afgekeurd",
                Details = $"RejectionForm ID {item.Id} voor Order {item.OrderId} aangemaakt met reden: {item.Reason}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        /// <summary>
        /// Wijzigt een afkeuringsformulier.
        /// </summary>
        /// <param name="id">ID van het formulier</param>
        /// <param name="item">Bijgewerkte formuliergegevens</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, RejectionForm item)
        {
            if (id != item.Id) return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Afkeuring aangepast",
                Details = $"RejectionForm ID {item.Id} aangepast voor Order {item.OrderId}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verwijdert een afkeuringsformulier.
        /// </summary>
        /// <param name="id">ID van het formulier</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.RejectionForms.FindAsync(id);
            if (item == null) return NotFound();

            _context.RejectionForms.Remove(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Afkeuring verwijderd",
                Details = $"RejectionForm ID {item.Id} verwijderd voor Order {item.OrderId}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

