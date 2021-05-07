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
            Debug.WriteLine("hey");
            int id = getUserId();
            if (User.IsInRole("Student"))
            {
                Debug.WriteLine("student");
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
                return View(res);
            }
            else if(User.IsInRole("Faculty"))
            {
                Debug.WriteLine("Faculty");
                var classes = _context.Events.ToList().FindAll(x => x.FacultyId == id);
                Debug.WriteLine(classes.Count());
                return View(classes);
            }
            else
            {
                Debug.WriteLine("admin");
                return View( _context.Events.ToList());
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
        [Authorize]
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End,Subject,Color,DescriptionFacultyId,IsExam")] Events course)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courses = await _context.Events.FindAsync(id);
            _context.Events.Remove(courses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeTableExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
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
