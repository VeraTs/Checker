using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
    [Route("ToDos")]
    [ApiController]
    public class TodosConsoleController : ControllerBase
    {
        private readonly Models.MyDatabaseContext _context;
        public TodosConsoleController(Models.MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: /Todos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todo.ToListAsync();
        }
    }
}
