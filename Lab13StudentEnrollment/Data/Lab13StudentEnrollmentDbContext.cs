using Microsoft.EntityFrameworkCore;
using Lab13StudentEnrollment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab13StudentEnrollment.Data
{
    public class Lab13StudentEnrollmentDbContext : DbContext
    {

        public Lab13StudentEnrollmentDbContext(DbContextOptions<Lab13StudentEnrollmentDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students{ get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
