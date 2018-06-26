using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Lab13StudentEnrollment.Models;
using Microsoft.EntityFrameworkCore;
using Lab13StudentEnrollment.Data;
using System.Linq;
using Lab13StudentEnrollment.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ControllerTest
{
    public class CourseController
    {
        [Fact]
        public async void CourseControllerCanCreate()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                Course course = new Course();
                course.CourseID = "cs101";
                course.Description = "Basic computer science class";
                //Act
                await context.Courses.AddAsync(course);
                await context.SaveChangesAsync();

                var results = context.Courses.Where(c => c.CourseID == "cs101");
                //Assert
                Assert.Equal(1, results.Count());
            }
        }

        [Fact]
        public void CourseGetterTest()
        {
            Course course = new Course();
            course.CourseID = "cs101";
            course.Description = "Basic computer science class";

            Assert.Equal("cs101", course.CourseID);

            string update = "Not Basic computer science class";
            course.CourseID = "cs201";
            course.Description = update;
            Assert.Equal("cs201", course.CourseID);
            Assert.Equal(update, course.Description);
        }

        [Fact]
        public void CheckThatTheIndexReturnsBasedOnSearch()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                Course course1 = new Course();
                course1.CourseID = "cs101";
                course1.Description = "Basic computer science class";

                Course course2 = new Course();
                course2.CourseID = "cs201";
                course2.Description = "Intermediate computer science class";

                CoursesController cc = new CoursesController(context);
                var x = cc.Create(course1);
                x = cc.Create(course2);
                

                var results = cc.Index("cs101").IsCompletedSuccessfully;
                Assert.True(results);

            }
        }

        [Fact]
        public void DetailsAreViewedThroughViewModel()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                Course course = new Course();
                course.CourseID = "cs101";
                course.Description = "Basic computer science class";

                CoursesController cc = new CoursesController(context);
                var x = cc.Create(course);
                var retrieve = context.Courses.Where(c => c.CourseID == "cs101").ToList();

                ViewResult current = (ViewResult)cc.Details(retrieve[0].ID).Result;
                CourseDetailsViewModel test = (CourseDetailsViewModel)current.Model;

                Assert.Equal(course.Description, test.Course.Description);
            }
        }

        [Fact]
        public void DetailsCanBeUpdatedThroughViewModel()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                Course course = new Course();
                course.CourseID = "cs101";
                course.Description = "Basic computer science class";

                CoursesController cc = new CoursesController(context);
                var x = cc.Create(course);
                var retrieve = context.Courses.Where(c => c.CourseID == "cs101").ToList();

                ViewResult current = (ViewResult)cc.Details(retrieve[0].ID).Result;
                CourseDetailsViewModel test = (CourseDetailsViewModel)current.Model;

                Assert.Equal("cs101", test.Course.CourseID);
                course.CourseID = "cs202";
                x = cc.Update(1, course);

                retrieve = context.Courses.Where(c => c.CourseID == "cs202").ToList();
                ViewResult update = (ViewResult)cc.Details(retrieve[0].ID).Result;
                test = (CourseDetailsViewModel)update.Model;

                Assert.Equal("cs202", test.Course.CourseID);
            }
        }

        [Fact]
        public void CourseCanBeDelete()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                Course course = new Course();
                course.CourseID = "cs101";
                course.Description = "Basic computer science class";

                CoursesController cc = new CoursesController(context);
                var x = cc.Create(course);
                var retrieve = context.Courses.Where(c => course.CourseID == "cs101").ToList();

                ViewResult current = (ViewResult)cc.Details(retrieve[0].ID).Result;
                CourseDetailsViewModel test = (CourseDetailsViewModel)current.Model;

                Assert.Equal("cs101", test.Course.CourseID);

                x = cc.Delete(test.Course.ID);
                retrieve = context.Courses.Where(c => c.CourseID == "cs101").ToList();
                Assert.Empty(retrieve);

            }
        }
    }
}
