using API.Models;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PicklistsController : ControllerBase
{
    private readonly SQLServerDatabaseContext _context;

    public PicklistsController(SQLServerDatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Picklist>>> GetAll() =>
        await _context.Picklists.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Picklist>> Get(int id)
    {
        var item = await _context.Picklists.FindAsync(id);
        return item == null ? NotFound() : item;
    }

    [HttpPost]
    public async Task<ActionResult<Picklist>> Post(Picklist item)
    {
        _context.Picklists.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Picklist item)
    {
        if (id != item.Id) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.Picklists.FindAsync(id);
        if (item == null) return NotFound();
        _context.Picklists.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
