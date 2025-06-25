using API.Data;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// API-controller voor het beheren van leveringen aan klanten.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public DeliveryController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetAll() =>
            await _context.Delivery.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> Get(int id)
        {
            var item = await _context.Delivery.FindAsync(id);
            return item == null ? NotFound() : item;
        }

        [HttpPost]
        public async Task<ActionResult<Delivery>> Post(Delivery item)
        {
            _context.Delivery.Add(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Levering aangemaakt",
                Details = $"Levering ID: {item.Id} aangemaakt met status {item.Status}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Delivery item)
        {
            if (id != item.Id) return BadRequest();

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Levering aangepast",
                Details = $"Levering ID: {item.Id} bijgewerkt naar status {item.Status}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Delivery.FindAsync(id);
            if (item == null) return NotFound();

            _context.Delivery.Remove(item);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = item.OrderId,
                Timestamp = DateTime.UtcNow,
                Activity = "Levering verwijderd",
                Details = $"Levering ID: {item.Id} verwijderd"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
