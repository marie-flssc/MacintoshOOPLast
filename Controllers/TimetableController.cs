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
            return View( _context.Courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeTable = await _context.Courses
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
        public async Task<IActionResult> Create([Bind("Id,Start,End,Name,Color")] StudentToClass timeTable)
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

            var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }
            return View(courses);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Start,End,Name,Color")] Courses course)
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

        //TODO DELETE STUDENTTOCLASSASWELL
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timeTable = await _context.Courses
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
            var courses = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(courses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeTableExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
