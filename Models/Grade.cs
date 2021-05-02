﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Models
{
    public class Grade
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
        public int StudentId { get; set; }
        public int Result { get; set; }
        public int coef { get; set; }
        public Subj subject { get; set; }
    }
}
