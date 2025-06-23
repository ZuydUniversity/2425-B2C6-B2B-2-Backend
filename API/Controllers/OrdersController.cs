using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public OrdersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
                return NotFound();

            return order;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            if (!CustomerExists(order.CustomerId) || !ProductExists(order.ProductId))
                return BadRequest("Invalid Customer or Product ID.");

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

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (!CustomerExists(order.CustomerId) || !ProductExists(order.ProductId))
                return BadRequest("Invalid Customer or Product ID.");

            if (order.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

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

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
                return NotFound();

            try
            {
                _context.Order.Remove(order);
                await _context.SaveChangesAsync();

                _context.EventLogs.Add(new EventLog
                {
                    OrderId = order.Id,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Order verwijderd",
                    Details = $"Order ID {order.Id} verwijderd door gebruiker of systeem"
                });

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Verwijderen van order {id} mislukt: {ex.Message}");
                return StatusCode(500, $"Interne fout bij verwijderen van order {id}");
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

        private bool CustomerExists(string username)
        {
            return _context.Customer.Any(e => e.Username == username);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
