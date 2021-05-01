using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OOP_CA_Macintosh.Data;
using OOP_CA_Macintosh.DTO;
using OOP_CA_Macintosh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Controllers
{
    [Authorize]
    public class TimetableController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<TimetableController> _logger;

        public TimetableController(Context context, ILogger<TimetableController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_context.Timetable.ToList());
        }


        [Authorize(Roles = AccessLevel.Faculty)]
        public IActionResult SeeStudentTimetable(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var time = GetCourses(id);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }

        
        [Authorize]
        public IActionResult SeeOwnTimetable()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.User.ToList().Find(x => x.Username.Equals(userId));
            int id = user.Id;
            var time = GetCourses(id);
            if (time == null)
            {
                return NotFound();
            }

            return View(time);
        }


        public List<Courses> GetCourses(int? id)
        {
            List<Courses> res = new List<Courses> { };

            var time = _context.Timetable;
            if(id != null)
            {
                foreach (StudentToClass classe in time)
                {
                    if (classe.StudentId == id.Value)
                    {
                        var tt = _context.Courses.ToList().Find(x => x.Id == classe.Course);
                        res.Add(tt);
                    }
                }
            }
            return res;
        }

        public JsonResult GetEvents()
        {
            var user = _context.User.FirstOrDefault(x => x.Username == User.Identity.Name);
            var events = _context.Timetable.Where(x => x.StudentId == user.Id);
            return new JsonResult(new { Data = events });
        }
    }
}
