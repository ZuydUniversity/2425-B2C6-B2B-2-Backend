using API.Models;
using API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// API-controller voor kwaliteitscontrole van producten.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QualityControlController : ControllerBase
    {
        private readonly SQLServerDatabaseContext _context;

        public QualityControlController(SQLServerDatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<QualityControl>> Post(QualityControl item)
        {
            _context.QualityControl.Add(item);
            await _context.SaveChangesAsync();

            string status = item.IsApproved ? "goedgekeurd" : "afgekeurd";
            _context.EventLogs.Add(new EventLog
            {
                Id = item.ProductId,
                Timestamp = DateTime.UtcNow,
                Activity = "Productcontrole uitgevoerd",
                Details = $"Product {item.ProductId} {status} - opmerkingen: {item.Comments}"
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Post), new { id = item.Id }, item);
        }
    }

}
