using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpeditionController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public ExpeditionController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Expedition
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expedition>>> GetExpeditions()
        {
            return await _context.Expeditions.ToListAsync();
        }

        // GET: api/Expedition/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expedition>> GetExpedition(int id)
        {
            var expedition = await _context.Expeditions.FindAsync(id);

            if (expedition == null)
            {
                return NotFound();
            }

            return expedition;
        }

        // POST: api/Expedition
        [HttpPost]
        public async Task<ActionResult<Expedition>> PostExpedition(Expedition expedition)
        {
            _context.Expeditions.Add(expedition);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpedition), new { id = expedition.Id }, expedition);
        }

        // PUT: api/Expedition/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpedition(int id, Expedition expedition)
        {
            if (id != expedition.Id)
            {
                return BadRequest();
            }

            _context.Entry(expedition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpeditionExists(id))
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

        // DELETE: api/Expedition/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpedition(int id)
        {
            var expedition = await _context.Expeditions.FindAsync(id);
            if (expedition == null)
            {
                return NotFound();
            }

            _context.Expeditions.Remove(expedition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpeditionExists(int id)
        {
            return _context.Expeditions.Any(e => e.Id == id);
        }
    }
}
