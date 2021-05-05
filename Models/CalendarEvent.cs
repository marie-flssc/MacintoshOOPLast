using Microsoft.EntityFrameworkCore;
using System;

namespace OOP_CA_Macintosh.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Color { get; set; }
        public bool IsExam { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int FacultyId { get; set; }
    }
}
