using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OOP_CA_Macintosh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Controllers
{
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
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public List<int> WhichStudentsMissedClass(int id)
        {
            throw new NotImplementedException();
        }

        public void ChangeAttendance(bool present, int id)
        {
            throw new NotImplementedException();
        }
    }
}
