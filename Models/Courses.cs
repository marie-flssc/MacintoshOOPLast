using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Models
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
    public class Courses
    {
       
        [Key]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public Subj Subject { get; set; }
        public int Length { get; set; }
        public bool IsExam { get; set; }
        public int FacultyId { get; set; }
        public DateTime End { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

    }
}
