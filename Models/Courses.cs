using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Models
{
    public class Courses
    {
        public enum Subj
        {
            IT,
            ENGLISH,
            MATH,
            PHYSICS,
            HISTORY,
            GEOGRAPHY
        }

        public int Id { get; set; }
        public string CourseId { get; set; }
        public DateTime Time { get; set; }
        public List<int> StudentsId { get; set; }
        public Subj Subject { get; set; }
        public int Length { get; set; }
        public bool IsExam { get; set; }
        
    }
}
