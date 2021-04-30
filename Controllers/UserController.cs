using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using OOP_CA_Macintosh.Models;
using OOP_CA_Macintosh.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OOP_CA_Macintosh.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace OOP_CA_Macintosh.Controllers
{
    public class UserController : Controller
    {
        private readonly Context _context;
        public UserController(
            Context context)
        {
            _context = context;
        }


        [HttpGet("Login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Validate(string username, string password, string returnUrl)
        {
            /*var user = await _context.User
                .FirstOrDefaultAsync(m => m.Username == username);
            if(user!=null && user.password == password)
            {
                return Ok();
            }*/
            if (username == "bob" && password == "lol")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(returnUrl);
            }
            return BadRequest();
        }

        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }

    }
}