using API.Models;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ApprovalFormsController : ControllerBase
{
    private readonly SQLServerDatabaseContext _context;

    public ApprovalFormsController(SQLServerDatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApprovalForm>>> GetAll() =>
        await _context.ApprovalForms.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<ApprovalForm>> Get(int id)
    {
        var item = await _context.ApprovalForms.FindAsync(id);
        return item == null ? NotFound() : item;
    }

    [HttpPost]
    public async Task<ActionResult<ApprovalForm>> Post(ApprovalForm item)
    {
        _context.ApprovalForms.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ApprovalForm item)
    {
        if (id != item.Id) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ApprovalForms.FindAsync(id);
        if (item == null) return NotFound();
        _context.ApprovalForms.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
