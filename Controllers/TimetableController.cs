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
using Microsoft.EntityFrameworkCore;
using static OOP_CA_Macintosh.Utils.timeTableUtils;
using System.Diagnostics;

namespace OOP_CA_Macintosh.Controllers
{
    [Authorize]
   public class TimetableController : Controller
    {
        private readonly Context _context;

        public TimetableController(Context context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            int id = getUserId();
            if (User.IsInRole("Student"))
            {
                var studtoclass = _context.StudentToClass.ToList().FindAll(x => x.StudentId.Equals(id));
                var res = new List<Events>();
                var courseId = new List<int>();
                foreach (StudentToClass chr in studtoclass)
                {
                    courseId.Add(chr.Course);
                }
                foreach (Events ev in _context.Events.ToList())
                {
                    //if they are here we add the corresponding student
                    if (courseId.Contains(ev.Id))
                    {
                        res.Add(ev);
                    }
                }
                List<Events> test = res;
                TempData["All"] = test;
                return View(RelevantCourses(res));
            }
            else if(User.IsInRole("Faculty"))
            {
                var classes = _context.Events.ToList().FindAll(x => x.FacultyId == id);
                TempData["All"] = classes;
                return View(RelevantCourses(classes));
            }
            else
            {
                var res = _context.Events.ToList();
                List<Events> test = res;
                TempData["All"] = test;
                return View(RelevantCourses(res));
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeTable = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeTable == null)
            {
                return NotFound();
            }

            return View(timeTable);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Start,End,Subject,Color,Description,FacultyId,IsExam")] Events timeTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(timeTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(timeTable);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses = await _context.Events.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }
            return View(courses);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =  "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End,Subject,Color,Description,FacultyId,IsExam")] Events course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeTableExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeTable = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeTable == null)
            {
                return NotFound();
            }

            return View(timeTable);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =  "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courses = await _context.Events.FindAsync(id);
            _context.Events.Remove(courses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> RegisterToCourse(int? id)
        {
            Debug.WriteLine("1er");
            if (id == null)
            {
                return NotFound();
            }
           

            var timeTable = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeTable == null)
            {
                return NotFound();
            }

            bool register = false;
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =  "Student")]
        public async Task<IActionResult> RegisterToCourse(int id)
        {

                StudentToClass st = new StudentToClass { Course = id, StudentId = getUserId() };
                _context.StudentToClass.Add(st);
                await _context.SaveChangesAsync();
            return RedirectToAction("AllCourses", "Timetable");
        }

        [Authorize(Roles = "Student")]
        public IActionResult AllCourses()
        {
            var studtoclass = _context.StudentToClass.ToList().FindAll(x => x.StudentId.Equals(getUserId()));
            var courseId = new List<int>();
            foreach (StudentToClass chr in studtoclass)
            {
                courseId.Add(chr.Course);
            }

            var res = RelevantCourses(_context.Events.ToList());
            foreach (Events ev in res.ToList())
            {
                if (courseId.Contains(ev.Id))
                {
                    res.Remove(ev);
                }
            }
            return View(res);
        }


        private bool TimeTableExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        private List<Events> RelevantCourses(List<Events> lis)
        {
            DateTime now = DateTime.Now;
            List<Events> res = new List<Events>();
            foreach (Events ev in lis.ToList())
            {
                var d = ev.End;
                if(DateTime.Compare(d,now)>=0)
                {
                    res.Add(ev);
                }
            }
            return res;
        }


        private int getUserId()
        {
            try
            {
                String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                User user = _context.User.ToList().Find(x => x.Username.Equals(userId));
                return user.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
