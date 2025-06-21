using API.Models;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RejectionFormsController : ControllerBase
{
    private readonly SQLServerDatabaseContext _context;

    public RejectionFormsController(SQLServerDatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RejectionForm>>> GetAll() =>
        await _context.RejectionForms.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<RejectionForm>> Get(int id)
    {
        var item = await _context.RejectionForms.FindAsync(id);
        return item == null ? NotFound() : item;
    }

    [HttpPost]
    public async Task<ActionResult<RejectionForm>> Post(RejectionForm item)
    {
        _context.RejectionForms.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, RejectionForm item)
    {
        if (id != item.Id) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.RejectionForms.FindAsync(id);
        if (item == null) return NotFound();
        _context.RejectionForms.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
