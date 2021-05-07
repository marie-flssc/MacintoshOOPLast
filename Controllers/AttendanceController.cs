using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OOP_CA_Macintosh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(Context context, ILogger<AttendanceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_context.Attendances.ToList().FindAll(x=>x.StudentId == getUserId()));
        }

        [Authorize(Roles ="Faculty")]
        //Only for the professors courses, a faculty cannot see the attendance for classes that do not concern them
        public IActionResult SeeStudentAttendance(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var profcourses = _context.Events.ToList().FindAll(x => x.FacultyId == getUserId());
            var res = new List<int>();
            foreach(var i in profcourses)
            {
                res.Add(i.Id);
            }
            return View(_context.Attendances.ToList().FindAll(x => x.StudentId ==id && res.Contains(x.CourseId)));
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
