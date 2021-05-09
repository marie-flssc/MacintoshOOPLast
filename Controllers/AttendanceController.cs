using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OOP_CA_Macintosh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OOP_CA_Macintosh.DTO;
using OOP_CA_Macintosh.Models;

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return View(attendance);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AccessLevel.Faculty)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,Present,StudentId")] Attendance model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Attendances.ToList().Find(x => x.Id.Equals(id));
                if (user == null)
                {
                    return BadRequest(new { message = "This user does not exist." });
                }
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("SeeStudentAttendance", new { 
                    id = model.StudentId}); 
            }
            return View(model);
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
