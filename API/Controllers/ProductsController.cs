using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Beheert productgegevens in het systeem.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public ProductsController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt een lijst op van alle producten.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.ToListAsync();
        }

        /// <summary>
        /// Haalt één specifiek product op aan de hand van zijn ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            return product == null ? NotFound() : product;
        }

        /// <summary>
        /// Wijzigt een bestaand product.
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="product">Bijgewerkte productgegevens</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                _context.EventLogs.Add(new EventLog
                {
                    OrderId = null,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Product aangepast",
                    Details = $"Product ID {product.Id} aangepast: {product.Name}"
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id)) return NotFound();
                throw;
            }

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        /// <summary>
        /// Voegt een nieuw product toe aan de database.
        /// </summary>
        /// <param name="product">Nieuwe productgegevens</param>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
                Timestamp = DateTime.UtcNow,
                Activity = "Product aangemaakt",
                Details = $"Nieuw product toegevoegd: {product.Name} (ID: {product.Id})"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        /// <summary>
        /// Verwijdert een product op basis van ID.
        /// </summary>
        /// <param name="id">Product ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return NotFound();

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
                Timestamp = DateTime.UtcNow,
                Activity = "Product verwijderd",
                Details = $"Product ID {product.Id} verwijderd: {product.Name}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}

