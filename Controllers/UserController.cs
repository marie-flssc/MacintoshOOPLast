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
using System.Diagnostics;

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


        [Authorize(Roles = "Faculty, Admin")]
        public async Task<IActionResult> Profile(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(int? id, String returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (id == null)
            {
                return NotFound();
            }

            var fee = await _context.User.FindAsync(id);
            if (fee == null)
            {
                return NotFound();
            }
            return View(fee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,Email,Username,Contact")] UpdateModel model, String returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = _context.User.ToList().Find(x => x.Id.Equals(id));
                if (user == null)
                {
                    return BadRequest(new { message = "This user does not exist." });
                }
                Console.WriteLine("la" + model.FirstName);
                Console.WriteLine("catain" + model.LastName);
                Console.WriteLine(model.Email);
                Console.WriteLine(model.Username);

                if ((string.IsNullOrEmpty(model.FirstName)) || (string.IsNullOrEmpty(model.LastName))
                    || (string.IsNullOrEmpty(model.Email)) || (string.IsNullOrEmpty(model.Username)))
                {
                    return BadRequest(new { message = "You did not specify everything." });
                }

                
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Username = model.Username;
                user.Contact = model.Contact;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Profile", new { 
                    id = id}); 
            }
            return View(model);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Username,Password,Role,Contact")] RegisterModel model)
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
                return RedirectToAction(nameof(Secured));
            }
            return RedirectToAction("Login", "User");
        }


        [HttpGet("SeeClass")]
        [Authorize(Roles = AccessLevel.Faculty)]
        public IActionResult SeeClass()
        {
            var res = new List<User>();
            int id = getUserId();
            //We get all classes of the professor
            var classe = _context.Events.ToList().FindAll(x => x.FacultyId.Equals(id));
            //here is the list where we keep all the ids of the classes of the professor
            var courseId = new List<int>();
            foreach(Events chr in classe) 
            {
                courseId.Add(chr.Id);
            }
            //We get all the "students to class" (the links from the students to their classes)
            var studenttoclass = _context.StudentToClass.ToList();
            //We compare each id with the ids in the classes of the professor
            foreach (StudentToClass st in studenttoclass)
            {
                //if they are here we add the corresponding student
                if(courseId.Contains(st.Course))
                {
                    res.Add(_context.User.ToList().Find(x => x.Id.Equals(st.StudentId)));
                }
            }
            return View(res);
        }

        [HttpGet("GetAllTeachers")]
        [Authorize(Roles = AccessLevel.Admin)]
        public IActionResult GetAllTeachers()
        {
            return View(_context.User.ToList().FindAll(x => x.Role.Equals("Faculty")));
        }

        [HttpGet("GetAllStudents")]
        [Authorize(Roles = AccessLevel.Admin)]
        public IActionResult GetAllStudents()
        {
            return View(_context.User.ToList().FindAll(x => x.Role.Equals("Student")));
        }

        [HttpGet]
        [Authorize(Roles = AccessLevel.Admin)]
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Home");
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
    

}