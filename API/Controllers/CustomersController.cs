using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public CustomersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            return await _context.Customer.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Process mining log
                _context.EventLogs.Add(new EventLog
                {
                    OrderId = 0,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Klantgegevens gewijzigd",
                    Details = $"Customer {customer.Username} (ID: {customer.Id}) aangepast"
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.Username }, customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            try
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(customer.Password);
                customer.Password = hashedPassword;
            }
            catch (Exception ex)
            {
                return BadRequest($"Error hashing password: {ex.Message}");
            }

            _context.Customer.Add(customer);
            try
            {
                await _context.SaveChangesAsync();

                // Process mining log
                _context.EventLogs.Add(new EventLog
                {
                    OrderId = 0,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Klant aangemaakt",
                    Details = $"Nieuwe klant {customer.Username} (ID: {customer.Id}) geregistreerd"
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.Username))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.Username }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            // Process mining log
            _context.EventLogs.Add(new EventLog
            {
                OrderId = 0,
                Timestamp = DateTime.UtcNow,
                Activity = "Klant verwijderd",
                Details = $"Klant {customer.Username} verwijderd uit het systeem"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(string username)
        {
            return _context.Customer.Any(e => e.Username == username);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}
