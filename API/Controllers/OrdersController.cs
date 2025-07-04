using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Beheert alle operaties rond bestellingen (Orders).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public OrdersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt alle orders op.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.Include(o => o.Customer).ToListAsync();
        }

        /// <summary>
        /// Haalt één specifieke order op via ID.
        /// </summary>
        /// <param name="id">Order-ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
            return order == null ? NotFound() : order;
        }

        /// <summary>
        /// Wijzigt een bestaande order.
        /// </summary>
        /// <param name="id">Order-ID</param>
        /// <param name="order">Gewijzigde order</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            if (!CustomerExists(order.CustomerId) || !ProductExists(order.ProductId))
                return BadRequest("Invalid Customer or Product ID.");

            if (order.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            if (order.OrderDate == null)
            {
                order.OrderDate = DateTime.UtcNow;
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                _context.EventLogs.Add(new EventLog
                {
                    OrderId = order.Id,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Order aangepast",
                    Details = $"Order ID {order.Id} aangepast door gebruiker of systeem"
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                else
                    throw;
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, new { order.Id });
        }

        /// <summary>
        /// Maakt een nieuwe order aan.
        /// </summary>
        /// <param name="order">Nieuwe order</param>
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (!CustomerExists(order.CustomerId) || !ProductExists(order.ProductId))
                return BadRequest("Invalid Customer or Product ID.");

            if (order.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            if (order.OrderDate == null)
            {
                order.OrderDate = DateTime.UtcNow;
            }

            try
            {
                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                Console.WriteLine($"[DEBUG] Order toegevoegd met ID: {order.Id}");

                _context.EventLogs.Add(new EventLog
                {
                    OrderId = order.Id,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Order aangemaakt",
                    Details = $"Nieuwe order ID {order.Id} voor klant {order.CustomerId}, product {order.ProductId}, aantal {order.Quantity}"
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOrder", new { id = order.Id }, new { order.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] POST Order mislukt: {ex.Message}");
                return StatusCode(500, "Interne serverfout bij aanmaken order.");
            }
        }

        /// <summary>
        /// Verwijdert een bestaande order.
        /// </summary>
        /// <param name="id">Order-ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
                return NotFound();

            try
            {
                _context.EventLogs.Add(new EventLog
                {
                    OrderId = order.Id,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Order verwijderd",
                    Details = $"Order ID {order.Id} verwijderd door gebruiker of systeem"
                });
                await _context.SaveChangesAsync();

                _context.Order.Remove(order);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Verwijderen van order {id} mislukt: {ex.Message}");
                return StatusCode(500, $"Interne fout bij verwijderen van order {id}");
            }
        }

        private bool OrderExists(int id) =>
            _context.Order.Any(e => e.Id == id);

        private bool CustomerExists(int id) =>
            _context.Customer.Any(e => e.Id == id);

        private bool ProductExists(int id) =>
            _context.Product.Any(e => e.Id == id);
    }
}

