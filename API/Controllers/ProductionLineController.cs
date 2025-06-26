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

    /// <summary>
    /// Haalt alle productielijnen op.
    /// </summary>
    /// <returns>Lijst van alle geregistreerde productielijnen.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductionLine>>> GetAll() =>
        await _context.ProductionLines.ToListAsync();

    /// <summary>
    /// Voegt een nieuwe productielijn toe.
    /// </summary>
    /// <param name="item">De productielijn die aangemaakt moet worden.</param>
    /// <returns>De nieuw aangemaakte productielijn met bijbehorend ID.</returns>
    [HttpPost]
    public async Task<ActionResult<ProductionLine>> Post(ProductionLine item)
    {
        _context.ProductionLines.Add(item);
        await _context.SaveChangesAsync();

        // Process mining log
        _context.EventLogs.Add(new EventLog
        {
            Timestamp = DateTime.UtcNow,
            Activity = "Productielijn geregistreerd",
            Details = $"Productielijn: {item.Name} actief: {item.IsActive}"
        });
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Post), new { id = item.Id }, item);
    }
}

