using API.Models;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// API-controller voor het beheren van goedkeuringsformulieren (ApprovalForms).
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ApprovalFormsController : ControllerBase
{
    private readonly SQLServerDatabaseContext _context;

    /// <summary>
    /// Constructor voor dependency injection van de databasecontext.
    /// </summary>
    public ApprovalFormsController(SQLServerDatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Haalt alle goedkeuringsformulieren op.
    /// </summary>
    /// <returns>Lijst van alle ApprovalForms</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApprovalForm>>> GetAll() =>
        await _context.ApprovalForms.ToListAsync();

    /// <summary>
    /// Haalt één goedkeuringsformulier op op basis van ID.
    /// </summary>
    /// <param name="id">De ID van het formulier</param>
    /// <returns>ApprovalForm object</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApprovalForm>> Get(int id)
    {
        var item = await _context.ApprovalForms.FindAsync(id);
        return item == null ? NotFound() : item;
    }

    /// <summary>
    /// Voegt een nieuw goedkeuringsformulier toe.
    /// </summary>
    /// <param name="item">ApprovalForm object</param>
    /// <returns>Het aangemaakte formulier</returns>
    [HttpPost]
    public async Task<ActionResult<ApprovalForm>> Post(ApprovalForm item)
    {
        _context.ApprovalForms.Add(item);
        await _context.SaveChangesAsync();

        _context.EventLogs.Add(new EventLog
        {
            OrderId = item.OrderId,
            Timestamp = DateTime.UtcNow,
            Activity = "Order goedgekeurd",
            Details = $"ApprovalForm ID: {item.Id} goedgekeurd op {item.DateApproved}"
        });
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    /// <summary>
    /// Wijzigt een bestaand goedkeuringsformulier.
    /// </summary>
    /// <param name="id">De ID van het formulier</param>
    /// <param name="item">Het gewijzigde ApprovalForm object</param>
    /// <returns>Geen inhoud bij succes</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ApprovalForm item)
    {
        if (id != item.Id) return BadRequest();

        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        _context.EventLogs.Add(new EventLog
        {
            OrderId = item.OrderId,
            Timestamp = DateTime.UtcNow,
            Activity = "Goedkeuringsformulier aangepast",
            Details = $"ApprovalForm ID: {item.Id} bijgewerkt"
        });
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Verwijdert een goedkeuringsformulier.
    /// </summary>
    /// <param name="id">De ID van het formulier</param>
    /// <returns>Geen inhoud bij succes</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ApprovalForms.FindAsync(id);
        if (item == null) return NotFound();

        _context.ApprovalForms.Remove(item);
        await _context.SaveChangesAsync();

        _context.EventLogs.Add(new EventLog
        {
            OrderId = item.OrderId,
            Timestamp = DateTime.UtcNow,
            Activity = "Goedkeuringsformulier verwijderd",
            Details = $"ApprovalForm ID: {item.Id} verwijderd"
        });
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

