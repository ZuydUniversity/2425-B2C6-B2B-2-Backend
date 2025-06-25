using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    /// <summary>
    /// Beheert inkooporders in het systeem.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public PurchaseOrdersController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haalt een lijst op van alle inkooporders.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrders()
        {
            return await _context.PurchaseOrders.ToListAsync();
        }

        /// <summary>
        /// Haalt een specifieke inkooporder op op basis van ID.
        /// </summary>
        /// <param name="id">Inkooporder ID</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            return order == null ? NotFound() : order;
        }

        /// <summary>
        /// Voegt een nieuwe inkooporder toe.
        /// </summary>
        /// <param name="order">Gegevens van de inkooporder</param>
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(PurchaseOrder order)
        {
            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
                Timestamp = DateTime.UtcNow,
                Activity = "Inkooporder aangemaakt",
                Details = $"Inkooporder ID {order.Id} aangemaakt voor Product {order.ProductId} bij leverancier {order.SupplierId}, Aantal {order.Quantity}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPurchaseOrder), new { id = order.Id }, order);
        }

        /// <summary>
        /// Wijzigt een bestaande inkooporder.
        /// </summary>
        /// <param name="id">ID van de inkooporder</param>
        /// <param name="order">Bijgewerkte gegevens</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrder(int id, PurchaseOrder order)
        {
            if (id != order.Id) return BadRequest();

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
                Timestamp = DateTime.UtcNow,
                Activity = "Inkooporder aangepast",
                Details = $"Inkooporder ID {order.Id} gewijzigd voor leverancier {order.SupplierId}"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verwijdert een inkooporder op basis van ID.
        /// </summary>
        /// <param name="id">Inkooporder ID</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            var order = await _context.PurchaseOrders.FindAsync(id);
            if (order == null) return NotFound();

            _context.PurchaseOrders.Remove(order);
            await _context.SaveChangesAsync();

            _context.EventLogs.Add(new EventLog
            {
                OrderId = null,
                Timestamp = DateTime.UtcNow,
                Activity = "Inkooporder verwijderd",
                Details = $"Inkooporder ID {order.Id} verwijderd uit het systeem"
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

