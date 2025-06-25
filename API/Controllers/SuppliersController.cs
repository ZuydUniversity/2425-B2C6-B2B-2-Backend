using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Beheert leveranciers in het systeem.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public SuppliersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt alle leveranciers op.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suppliers>>> GetSuppliers()
        {
            return await _context.Suppliers.ToListAsync();
        }

        /// <summary>
        /// Haalt een specifieke leverancier op.
        /// </summary>
        /// <param name="id">De ID van de leverancier</param>
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

        /// <summary>
        /// Wijzigt een bestaande leverancier.
        /// </summary>
        /// <param name="id">De ID van de leverancier</param>
        /// <param name="suppliers">De gewijzigde leverancier</param>
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

                _context.EventLogs.Add(new EventLog
                {
                    OrderId = null,
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

        /// <summary>
        /// Voegt een nieuwe leverancier toe.
        /// </summary>
        /// <param name="suppliers">De nieuwe leverancier</param>
        [HttpPost]
        public async Task<ActionResult<Suppliers>> PostSuppliers(Suppliers suppliers)
        {
            _context.Suppliers.Add(suppliers);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
                Timestamp = DateTime.UtcNow,
                Activity = "Leverancier toegevoegd",
                Details = $"Nieuwe leverancier toegevoegd: {suppliers.Name} (ID: {suppliers.Id})"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSuppliers", new { id = suppliers.Id }, suppliers);
        }

        /// <summary>
        /// Verwijdert een leverancier.
        /// </summary>
        /// <param name="id">De ID van de leverancier</param>
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

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
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

