using API.Models;
using API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// API-controller voor het beheren van onderdeelleveringen aan productielijn.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PartsDeliveryController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public PartsDeliveryController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<PartsDelivery>> Post(PartsDelivery item)
        {
            _context.PartsDelivery.Add(item);
            await _context.SaveChangesAsync();

            string activity = item.IsComplete ? "Correcte onderdelen levering" : "Incorrecte onderdelen levering";
            _context.EventLogs.Add(new EventLog
            {
                Timestamp = DateTime.UtcNow,
                Activity = activity,
                Details = $"PartsDelivery ID: {item.Id} status: {(item.IsComplete ? "volledig" : "incompleet")}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = item.Id }, item);
        }
    }

}
