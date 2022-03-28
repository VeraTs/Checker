using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreSqlDb.Hubs
{
    public class ToDosHub : Hub
    {
        private readonly Models.MyDatabaseContext _context;

        public ToDosHub(Models.MyDatabaseContext context)
        {
            _context = context;
        }

        // triggers the ReceiveList event for this particular client with the input of todos - the To Do list
        public async Task InitialToDos()
        {
            List<Todo> list = await _context.Todo.ToListAsync();
            foreach (Todo todo in list)
            {
                await Clients.Caller.SendAsync("ReceiveList", todo.Description, todo.CreatedDate);
            }
        }

        public async Task AddToDo(string desc, DateTime date)
        {
            Todo todo = new Todo();
            todo.Description = desc;
            todo.CreatedDate = date;
            _context.Add(todo);
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveList", todo.Description, todo.CreatedDate);
        }
    }
}
