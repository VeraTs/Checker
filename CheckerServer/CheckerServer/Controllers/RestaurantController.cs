using Microsoft.AspNetCore.Mvc;
using CheckerServer.Models;
using CheckerServer.Data;
using Microsoft.EntityFrameworkCore;
using CheckerServer.utils;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using FluentEmail.Smtp;
using FluentEmail.Core;

namespace CheckerServer.Controllers
{
    [Route("Restaurants")]
    [ApiController]
    public class RestaurantController : BasicDbController<Restaurant>
    {
        public RestaurantController(CheckerDBContext context)
            : base(context, context.Restaurants)
        { }

        protected override void updateItem(Restaurant existingItem, Restaurant updatedItem)
        {
            if (!string.IsNullOrEmpty(updatedItem.Name))
            {
                existingItem.Name = updatedItem.Name;
            }

            if (!string.IsNullOrEmpty(updatedItem.ContactName))
            {
                existingItem.ContactName = updatedItem.ContactName;
            }

            if (!string.IsNullOrEmpty(updatedItem.Phone))
            {
                existingItem.Phone = updatedItem.Phone;
            }
        }

        override internal async Task<ActionResult<IEnumerable<Restaurant>>> get()
        {
            var res = await r_Set
                .Include("ServingAreas")
                .Include("Lines")
                .Include("Menus.Dishes")
                .ToListAsync();

            return res;
        }

        override internal async Task<ActionResult<Restaurant>> getSpecific(int id)
        {
            var res = await r_Set
                .Include("ServingAreas")
                .Include("Lines")
                .Include("Menus.Dishes")
                .FirstOrDefaultAsync(d => d.ID == id);

            return res;
        }
    }
}
