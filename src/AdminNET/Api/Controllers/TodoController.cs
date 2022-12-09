using AdminNET.Api.Filters;
using AdminNET.Data;
using AdminNET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminNET.Api.Controllers;

[ApiController]
[Route("todos")]
[ServiceFilter(typeof(AuthFilter))]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ApplicationDbContext _context;

    public TodoController(ILogger<TodoController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]    
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        return await _context.Todos.ToListAsync();
    }
}