using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public bool Present { get; set; }
        public int StudentsId { get; set; }

    }
}
