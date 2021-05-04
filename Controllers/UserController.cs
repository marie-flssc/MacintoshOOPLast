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
using OOP_CA_Macintosh.DTO;

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
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Username == username);
            
            if (user != null && user.Password == password)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
                //TODO ADD HERE ALL THINGS IN DATABASE 37:00 ENVIRON
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect(returnUrl);
            }
            TempData["Error"] = "Either the username or the password is invalid";
            return Redirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        
        [Authorize]
        public IActionResult Secured()
        {
            return View();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AccessLevel.Admin)]
        public IActionResult Edit(int id, [Bind("Firstname,LastName,Email,Username")] UpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User.ToList().Find(x => x.Id.Equals(id));
                if (user == null)
                {
                    return BadRequest(new { message = "This user does not exist." });
                }

                if ((string.IsNullOrEmpty(model.FirstName)) || (string.IsNullOrEmpty(model.LastName))
                    || (string.IsNullOrEmpty(model.Email)) || (string.IsNullOrEmpty(model.Username)))
                {
                    return BadRequest(new { message = "You did not specify everything." });
                }

                
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Username = model.Username;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }


        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Username,Password,Role")] RegisterModel model)
        {
            User modelUser = new User();
            modelUser.FirstName = model.FirstName;
            modelUser.LastName = model.LastName;
            modelUser.Email = model.Email;
            modelUser.Password = model.Password;
            modelUser.Username = model.Username;
            modelUser.Role = model.Role;

            if (_context.User.ToList().FindAll(x => x.Username.Equals(modelUser.Username)).Count > 0)
            {
                TempData["Error"] = "Username is already taken";
                return Redirect(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                _context.Add(modelUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction("Login", "User");
        }


        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult GetAllStudents()
        {
            var res = new List<User>();
            int id = getUserId();
            List<User> student = _context.User.ToList().FindAll(x => x.Role.Equals("Student"));
            List<Courses> classe = _context.Courses.ToList().FindAll(x => x.FacultyId.Equals(id));
            var courseId = new List<int>();
            foreach(Courses chr in classe) 
            {
                courseId.Add(chr.Id);
            }
            var studenttoclass = _context.Timetable;
            foreach(StudentToClass st in studenttoclass)
            {
                if(courseId.Contains(st.Course))
                {
                    res.Add(student.Find(x => x.Id.Equals(st.StudentId)));
                }
            }
            return View(res);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult GetAllTeachers()
        {
            return View(_context.User.ToList().FindAll(x => x.Role.Equals("Faculty")));
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult GetUsers()
        {
            return View(_context.User);
        }


        private int getUserId()
        {
            try
            {
                String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _context.User.ToList().Find(x => x.Username.Equals(userId));
                return user.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }


    }
}