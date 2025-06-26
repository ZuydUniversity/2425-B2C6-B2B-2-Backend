using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// API-controller voor het beheren van klanten.
/// </summary>
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        /// <summary>
        /// Constructor met dependency injection.
        /// </summary>
        public CustomersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt alle klanten op.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            return await _context.Customer.ToListAsync();
        }

        /// <summary>
        /// Haalt een klant op via ID.
        /// </summary>
        /// <param name="id">Klant-ID</param>
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

        /// <summary>
        /// Wijzigt klantgegevens.
        /// </summary>
        /// <param name="id">Klant-ID</param>
        /// <param name="customer">Gewijzigde klantgegevens</param>
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
                    OrderId = null,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Klantgegevens gewijzigd",
                    Details = $"Customer {customer.Name} (ID: {customer.Id}) aangepast"
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

            return CreatedAtAction("GetCustomer", new { id = customer.Name }, customer);
        }

        /// <summary>
        /// Voegt een nieuwe klant toe.
        /// </summary>
        /// <param name="customer">Nieuwe klant</param>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            //try
            //{
            //    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(customer.Password);
            //    customer.Password = hashedPassword;
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest($"Error hashing password: {ex.Message}");
            //}

            _context.Customer.Add(customer);
            try
            {
                await _context.SaveChangesAsync();

                _context.EventLogs.Add(new EventLog
                {
                    OrderId = null,
                    Timestamp = DateTime.UtcNow,
                    Activity = "Klant aangemaakt",
                    Details = $"Nieuwe klant {customer.Name} (ID: {customer.Id}) geregistreerd"
                });
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.Name }, customer);
        }

        /// <summary>
        /// Verwijdert een klant.
        /// </summary>
        /// <param name="id">Klant-ID (string)</param>
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

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
                Timestamp = DateTime.UtcNow,
                Activity = "Klant verwijderd",
                Details = $"Klant {customer.Name} verwijderd uit het systeem"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(string username)
        {
            return _context.Customer.Any(e => e.Name == username);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Id == id);
        }
    }
}

