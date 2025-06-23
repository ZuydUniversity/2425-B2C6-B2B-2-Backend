using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public SuppliersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suppliers>>> GetSuppliers()
        {
            return await _context.Suppliers.ToListAsync();
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Suppliers>> GetSuppliers(int id)
        {
            var suppliers = await _context.Suppliers.FindAsync(id);

            if (suppliers == null)
            {
                return NotFound();
            }

            return suppliers;
        }

        // PUT: api/Suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSuppliers(int id, Suppliers suppliers)
        {
            if (id != suppliers.Id)
            {
                return BadRequest();
            }

            _context.Entry(suppliers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Process mining log
                _context.EventLogs.Add(new EventLog
                {
                    OrderId = 0,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Leverancier aangepast",
                    Details = $"Leverancier ID {suppliers.Id} aangepast: {suppliers.Name}"
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSuppliers", new { id = suppliers.Id }, suppliers);
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<ActionResult<Suppliers>> PostSuppliers(Suppliers suppliers)
        {
            _context.Suppliers.Add(suppliers);
            await _context.SaveChangesAsync();

            // Process mining log
            _context.EventLogs.Add(new EventLog
            {
                OrderId = 0,
                Timestamp = DateTime.UtcNow,
                Activity = "Leverancier toegevoegd",
                Details = $"Nieuwe leverancier toegevoegd: {suppliers.Name} (ID: {suppliers.Id})"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSuppliers", new { id = suppliers.Id }, suppliers);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuppliers(int id)
        {
            var suppliers = await _context.Suppliers.FindAsync(id);
            if (suppliers == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(suppliers);
            await _context.SaveChangesAsync();

            // Process mining log
            _context.EventLogs.Add(new EventLog
            {
                OrderId = 0,
                Timestamp = DateTime.UtcNow,
                Activity = "Leverancier verwijderd",
                Details = $"Leverancier ID {suppliers.Id} verwijderd: {suppliers.Name}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
