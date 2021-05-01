using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OOP_CA_Macintosh.Data;
using OOP_CA_Macintosh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(Context context, ILogger<CoursesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult Remove()
        {
            return View();
        }

        [HttpPut]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }

        public int AddCourse(Courses course)
        {
            throw new NotImplementedException();
        }

        public IActionResult AddNewCourse()
        {
            return View();
        }

        public IActionResult AddAction([Bind("Time,Subject,Length,IsExam")] Courses course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction("Index", "Courses");
        }

        //TODO ADD STUDENT
    }
}
