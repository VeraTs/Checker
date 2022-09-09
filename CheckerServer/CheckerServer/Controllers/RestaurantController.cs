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


        /*   [HttpPost]
           [Route("login")]
           public async Task<ActionResult<Restaurant>> userLogin([FromBody] User i_user)
           {
               if (ModelState.IsValid)
               {
                   // Use Input.Email and Input.Password to authenticate the user
                   // with your custom authentication logic.
                   //
                   // For demonstration purposes, the sample validates the user
                   // on the email address maria.rodriguez@contoso.com with 
                   // any password that passes model validation.



                   Restaurant rest = await r_DbContext.Restaurants
                       .Include("Menus")
                       .FirstOrDefaultAsync(r => r.Email.Equals(i_user.UserEmail) && r.Password.Equals(i_user.UserPassword));
                   if (rest == null || String.IsNullOrEmpty(rest.Email))
                   {
                       return BadRequest("UserName or Password is incorrect");
                   }
                   return rest;
               }

               return BadRequest();
           }

           [HttpPost]
           [Route("register")]
           public async Task<ActionResult<Restaurant>> userRegisteration(Restaurant item)
           {
               if (ModelState.IsValid && item != null)
               {
                   Restaurant rest = await r_DbContext.Restaurants.FirstOrDefaultAsync(r => r.Email.Equals( item.Email));
                   if (rest != null)
                   {
                       return BadRequest("The Email is already in use");
                   }
                   try
                   {
                       int res = (await DBSetHelper.AddHelper<Restaurant>(r_DbContext, item, r_Set)).Value;
                       if (res > 0)
                       {
                           // return the added item
                           sendAnEmailAsync(item.Email, item.Name);
                           return item;
                       }
                       else
                       {
                           // return null item
                           return BadRequest("DB error: cannot insert item");
                       }
                   }
                   catch (Exception ex)
                   {
                       return BadRequest(CRUDutils.onRejectFromDB(item));
                   }
               }
               else
               {
                   String msg = "";
                   if (item == null)
                   {
                       msg += "Invalid Syntax for Object\n";
                   }

                   if (!ModelState.IsValid)
                   {
                       msg += "Internal error";
                   }
                   return BadRequest(msg);
               }
           }

           private async Task sendAnEmailAsync(String i_Email,String i_RestuarantName)
           {
               var sender = new SmtpSender(() => new SmtpClient("smtp-mail.outlook.com")
               {
                   EnableSsl = true,
                   DeliveryMethod = SmtpDeliveryMethod.Network,
                   Port = 587,
                   UseDefaultCredentials = false,
                   Credentials = new NetworkCredential()
                   {
                       UserName = "CheckerApp@outlook.com",
                       Password = "checker123"
                   }
               }); ;

               Email.DefaultSender = sender;

               var email = await Email
                   .From("CheckerApp@outlook.com")
                   .To(i_Email, i_RestuarantName)
                   .Subject("Tanks for the registeration")
                   .Body("Welcome to Checker App where the world is greener")
                   .SendAsync();
           }*/
    }
}
