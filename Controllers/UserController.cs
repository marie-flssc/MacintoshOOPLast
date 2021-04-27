using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OOP_CA_Macintosh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Controllers
{
    [Authorize]
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class UserController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<UserController> _logger;

        
        public UserController(Context context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /*
        public IActionResult Index()
        {
            return View();
        }*/

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        /*
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }*/
        [HttpPost("Register")]
        public IActionResult Register()
        {
            return View();
        }

    }
}
