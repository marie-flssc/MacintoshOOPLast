using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OOP_CA_Macintosh.Models;

namespace OOP_CA_Macintosh.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<StudentToClass> StudentToClass { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<CalendarEvent> CalendarEvent { get; set; }

    }
}
