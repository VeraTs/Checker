using CheckerServer.Data;
using CheckerServer.Models;
using CheckerServer.utils;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace CheckerServer.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UsersController : BasicDbController<Restaurant>
    {

        public UsersController(CheckerDBContext context)
           : base(context, context.Restaurants)
        { }

        [HttpPost]
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
                Restaurant rest = await r_DbContext.Restaurants.FirstOrDefaultAsync(r => r.Email.Equals(item.Email));
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

        protected override void updateItem(Restaurant existingItem, Restaurant updatedItem)
        {
            throw new NotImplementedException();
        }

        private async Task sendAnEmailAsync(String i_Email, String i_RestuarantName)
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
        }
    }
}
