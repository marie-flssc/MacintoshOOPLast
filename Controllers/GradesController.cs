using Microsoft.AspNetCore.Mvc;
using OOP_CA_Macintosh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Remove()
        {
            return View();
        }
    }
}
