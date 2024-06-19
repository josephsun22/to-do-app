using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly ToDoContext _context;

    public ToDoItemsController(ToDoContext context)
    {
        _context = context;

        if (_context.ToDoItems.Count() == 0)
        {
            _context.ToDoItems.Add(new ToDoItem { Name = "Item1" });
            _context.SaveChanges();
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDoItems()
    {
        return await _context.ToDoItems.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItem>> GetToDoItem(int id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        return toDoItem;
    }

    [HttpPost]
    public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem toDoItem)
    {
        _context.ToDoItems.Add(toDoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetToDoItem), new { id = toDoItem.Id }, toDoItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutToDoItem(int id, ToDoItem toDoItem)
    {
        if (id != toDoItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(toDoItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteToDoItem(int id)
    {
        var toDoItem = await _context.ToDoItems.FindAsync(id);

        if (toDoItem == null)
        {
            return NotFound();
        }

        _context.ToDoItems.Remove(toDoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}