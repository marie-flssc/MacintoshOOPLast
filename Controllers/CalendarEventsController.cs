using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OOP_CA_Macintosh.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OOP_CA_Macintosh.Models;

namespace OOP_CA_Macintosh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventsController : ControllerBase
    {
        private readonly Context _context;

        public CalendarEventsController(Context context)
        {
            _context = context;
        }

        // GET: api/CalendarEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Events>>> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            return await _context.Events
                .Where(e => !((e.End <= start) || (e.Start >= end)))
                .ToListAsync();
        }

        // GET: api/CalendarEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Events>> GetCalendarEvent(int id)
        {
            var calendarEvent = await _context.Events.FindAsync(id);

            if (calendarEvent == null)
            {
                return NotFound();
            }

            return calendarEvent;
        }

        // PUT: api/CalendarEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarEvent(int id, CalendarEvent calendarEvent)
        {
            if (id != calendarEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(calendarEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarEventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CalendarEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Events>> PostCalendarEvent(Events calendarEvent)
        {
            _context.Events.Add(calendarEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.Id }, calendarEvent);
        }

        // DELETE: api/CalendarEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarEvent(int id)
        {
            var calendarEvent = await _context.Events.FindAsync(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }

            _context.Events.Remove(calendarEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CalendarEventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
