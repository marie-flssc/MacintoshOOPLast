using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Models
{
    public class StudentToClass
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int Course { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        public string Color { get; set; }

    }
}
