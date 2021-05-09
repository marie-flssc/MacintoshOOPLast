using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class FeesController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<FeesController> _logger;

        public FeesController(Context context, ILogger<FeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_context.Fees.ToList().FindAll(x=>x.StudentId == getUserId()));
        }

        public async Task<IActionResult> Pay(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fee = await _context.Fees.FindAsync(id);
            if (fee == null)
            {
                return NotFound();
            }
            return View(fee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AccessLevel.Student)]
        public async Task<IActionResult> Pay(int id, [Bind("Id,StudentId,AmountToPay,Payed,Name")] Fee feeresult)
        {
            if (ModelState.IsValid)
            {
                var fee = _context.Fees.ToList().Find(x => x.Id == id);
                fee.Payed = feeresult.Payed;
                _context.Update(fee);
                await _context.SaveChangesAsync();

               return RedirectToAction("Index", new { 
                    id = id});
            }
            return View(feeresult);
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Fees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
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

        private bool FeeExists(int id)
        {
            return _context.Fees.Any(e => e.Id == id);
        }
    }
}
