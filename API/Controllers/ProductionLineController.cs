using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// API-controller voor het beheren van productielijnen.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProductionLineController : ControllerBase
{
    private readonly SQLServerDatabaseContext _context;

    public ProductionLineController(SQLServerDatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductionLine>>> GetAll() =>
        await _context.ProductionLines.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<ProductionLine>> Post(ProductionLine item)
    {
        _context.ProductionLines.Add(item);
        await _context.SaveChangesAsync();
        _context.EventLogs.Add(new EventLog
        {
            Timestamp = DateTime.UtcNow,
            Activity = "Productielijn geregistreerd",
            Details = $"Productielijn: {item.LineName} actief: {item.IsActive}"
        });
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Post), new { id = item.Id }, item);
    }
}
