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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planning>>> GetAll() =>
            await _context.Planning.ToListAsync();

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
    }

}
