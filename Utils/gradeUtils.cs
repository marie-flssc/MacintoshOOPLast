using OOP_CA_Macintosh.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_CA_Macintosh.Utils
{
    public class gradeUtils
    {
        public static List<Grade> getGrade(int studentId, List<Grade> allGrade)
        {
            List<Grade> grades = new List<Grade> { };

            foreach (Grade grade in allGrade)
            {
                if (grade.StudentId.Equals(studentId))
                {
                    grades.Add(grade);
                }
            }
            return grades;
        }
    }

}