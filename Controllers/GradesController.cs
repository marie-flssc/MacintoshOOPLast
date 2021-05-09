using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOP_CA_Macintosh.Data;
using OOP_CA_Macintosh.DTO;
using OOP_CA_Macintosh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static OOP_CA_Macintosh.Utils.gradeUtils;

namespace OOP_CA_Macintosh.Controllers
{

    public class GradesController : Controller
    {
        private readonly Context _context;
        public GradesController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(getGrade(getUserId(), _context.Grades.ToList()));
        }

        public IActionResult SeeStudentGrades(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(getGrade(id.Value, _context.Grades.ToList()));
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TempData["id"] = "Please enter as student number : " + id.ToString();
            return View();
        }

        [Authorize(Roles = AccessLevel.Faculty)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id,[Bind("StudentId,Coef,Subject,Result")] Grade model)
        {
            TempData["id"] = "Please enter : " + id.ToString(); ;
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("SeeStudentGrades", new { 
                    id = model.StudentId});
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var grade = await _context.Grades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        [Authorize(Roles = AccessLevel.Faculty)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Grades.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AccessLevel.Faculty)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Subject,Result,Coef")] Grade model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("SeeStudentGrades", new { 
                    id = model.StudentId}); 
            }
            return View(model);
        }


        [Authorize(Roles = AccessLevel.Faculty)]
        public async Task<IActionResult> Remove(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Grades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AccessLevel.Faculty)]
        public async Task<IActionResult> RemoveConfirmed(int id)
        {
            var model = await _context.Grades.FindAsync(id);
            _context.Grades.Remove(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("SeeStudentGrades", new { 
                    id = model.StudentId});
        }

        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.Id == id);
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
