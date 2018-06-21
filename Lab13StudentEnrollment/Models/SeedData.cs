using System;
using Lab13StudentEnrollment.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Lab13StudentEnrollment.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Lab13StudentEnrollmentDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<Lab13StudentEnrollmentDbContext>>()))
            {
                if (context.Courses.Any() || context.Students.Any())
                {
                    return;
                }

                context.Courses.AddRange(
                    new Course
                    {
                        CourseID = "cs101",
                        Description = "Beginning Computer Science Class"
                    },
                    new Course
                    {
                        CourseID = "cs201",
                        Description = "Intermediate Computer Science Class"
                    },
                    new Course
                    {
                        CourseID = "cs301",
                        Description = "Advance Computer Science Class"
                    },
                    new Course
                    {
                        CourseID = "mt101",
                        Description = "Beginning Music Theory Class"
                    },
                    new Course
                    {
                        CourseID = "mt201",
                        Description = "Intermediate Music Theory Class"
                    },
                    new Course
                    {
                        CourseID = "mt301",
                        Description = "Advance Music Theory Class"
                    }
                   );
                context.SaveChanges();
                context.Students.AddRange(
                    new Student
                    {
                        Name = "Bob peterson",
                        Age = 19,
                        CourseID = "cs101",
                    },
                    new Student
                    {
                        Name = "Rosie Ale",
                        Age = 21,
                        CourseID = "cs201",
                    },
                    new Student
                    {
                        Name = "Samantha Paisley",
                        Age = 20,
                        CourseID = "cs301",
                    },
                    new Student
                    {
                        Name = "Nathan Orion",
                        Age = 20,
                        CourseID = "cs301",
                    },
                    new Student
                    {
                        Name = "Jason Howard",
                        Age = 21,
                        CourseID = "mt201",
                    },
                    new Student
                    {
                        Name = "Amelia Dale",
                        Age = 19,
                        CourseID = "mt201",
                    }
                   );
                context.SaveChanges();
            }
        }

    }
}
