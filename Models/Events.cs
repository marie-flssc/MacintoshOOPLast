using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Models
{
    public class Events
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
