using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;

namespace DotNetCoreSqlDb.Controllers
{
    [Route("JsonToDos")]
    [ApiController]
    public class TodosConsoleController : ControllerBase
    {
        private readonly Models.MyDatabaseContext _context;
        public TodosConsoleController(Models.MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: /JsonToDos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todo.ToListAsync();
        }

        // GET: JsonTodos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodos(int id)
        {
            var res = await _context.Todo
                .FirstOrDefaultAsync(m => m.ID == id);

            if(res == null)
            {
                return NotFound();
            }

            return res;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddToDo(Todo todo)
        {
            
            if (ModelState.IsValid && todo != null)
            {
                try
                {
                    _context.Todo.Add(todo);
                    return await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    String msg = "";
                    if(todo.ID != 0)
                    {
                        msg = "You cannot decide an item id on your own";
                    }
                    else
                    {
                        msg = "a server error has occured";
                    }
                    return BadRequest(msg);
                }
                

            }
            else
            {
                String msg = "";
                if(todo == null)
                {
                    msg += "Invalid Sytax for Object\n";
                }

                if (!ModelState.IsValid)
                {
                    msg += "Internal error";
                }
                return BadRequest(msg);
            }
        }
    }
}
