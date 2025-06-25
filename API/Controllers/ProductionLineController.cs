using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProductionLineController : ControllerBase
{
    private readonly SQLServerDatabaseContext _context;
    public ProductionLineController(SQLServerDatabaseContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductionLine>>> Get() =>
        await _context.ProductionLines.ToListAsync();

    [HttpPost]
    public async Task<ActionResult<ProductionLine>> Post(ProductionLine line)
    {
        _context.ProductionLines.Add(line);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = line.Id }, line);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ProductionLine line)
    {
        if (id != line.Id) return BadRequest();
        _context.Entry(line).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ProductionLines.FindAsync(id);
        if (item == null) return NotFound();
        _context.ProductionLines.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
