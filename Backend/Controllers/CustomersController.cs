using Data;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;


namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public CustomersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.Customers.ToList());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _context.Customers.Find(id);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Customer updated)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            customer.Name = updated.Name;
            customer.Username = updated.Username;
            customer.Password = updated.Password;

            _context.SaveChanges();
            return NoContent();
        }
    }
}
