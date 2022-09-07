using CheckerServer.Data;
using CheckerServer.Models;
using Microsoft.AspNetCore.Mvc;
using CheckerServer.utils;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace CheckerServer.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CheckerDBContext context;

        public UsersController(CheckerDBContext context)
        {
            this.context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin([FromBody]User i_user)
        {
            if (ModelState.IsValid)
            {
                // Use Input.Email and Input.Password to authenticate the user
                // with your custom authentication logic.
                //
                // For demonstration purposes, the sample validates the user
                // on the email address maria.rodriguez@contoso.com with 
                // any password that passes model validation.

                User? dbUser = context.Users.Where(u => u.Email == i_user.Email && u.Password == HashTool.hashPassword(i_user.Password)).FirstOrDefault();

                if (dbUser == null || String.IsNullOrEmpty(dbUser.Email))
                {
                    return BadRequest("UserName or Password is incorrect");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dbUser.Email),
                    new Claim("Email", dbUser.Email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok(new User() { Email = i_user.Email });
            }           

            return BadRequest();  
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> userRegistration([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                User? dbUser = context.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                if (dbUser != null && !String.IsNullOrEmpty(dbUser.Email))
                {
                    return BadRequest("User for this email already exists!");
                }

                context.Users.Add(new User() { Email = user.Email, Password = HashTool.hashPassword(user.Password) });
                try
                {
                    int res = await context.SaveChangesAsync();
                    if (res > 0)
                    {
                        return Ok("registration successful!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return BadRequest("An error occured");
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> OnGetAsync()
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    // Clear the existing external cookie
                    await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    return Ok("Log out successful!");
                }
                else
                {
                    return BadRequest("you are not currently logged in, and cannot log out");
                }
            }

            return BadRequest();
        }

    }
}
