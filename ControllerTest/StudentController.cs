using Lab13StudentEnrollment.Controllers;
using Microsoft.AspNetCore.Mvc;
using Lab13StudentEnrollment.Data;
using Lab13StudentEnrollment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace ControllerTest
{
    public class StudentController
    {
        [Fact]
        public async void StudentsControllerCanCreate()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                //Arrange
                Student student = new Student();
                student.Name = "Bob";
                student.Age = 24;
                student.CourseID = "cs101";
                //Act
                await context.Students.AddAsync(student);
                await context.SaveChangesAsync();

                var results = context.Students.Where(s => s.Name == "Bob");
                //Assert
                Assert.Equal(1, results.Count());
            }
        }

        [Fact]
        public void DatabaseCanSave()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                //Arrange
                Student student = new Student();
                student.Name = "Bob";
                student.Age = 24;
                student.CourseID = "cs201";
                //Act
                StudentsController sc = new StudentsController(context);
                var x = sc.Create(student);
                var results = context.Students.Where(s => s.Name == "Bob");
                //Assert
                Assert.Equal(1, results.Count());
            }
        }

        [Fact]
        public void StudentNameGetterTest()
        {
            //Arrange
            Student student = new Student();
            student.Name = "Bob";
            student.Age = 24;
            student.CourseID = "cs201";
            //Act
            student.Name = "Joe";
            student.Age = 30;
            student.CourseID = "bfa123";
            Assert.Equal("Joe", student.Name);
            Assert.Equal(30, student.Age);
            Assert.Equal("bfa123", student.CourseID);
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
                Student student = new Student();
                student.Name = "Bob";
                student.Age = 24;
                student.CourseID = "cs201";

                Student student2 = new Student();
                student.Name = "Joe";
                student.Age = 22;
                student.CourseID = "cs301";

                StudentsController sc = new StudentsController(context);
                var x = sc.Create(student);
                x = sc.Create(student2);

                var results = sc.Index("").IsCompletedSuccessfully;
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
                Student student = new Student();
                student.Name = "Bob";
                student.Age = 24;
                student.CourseID = "cs201";

                StudentsController sc = new StudentsController(context);
                var x = sc.Create(student);
                var retrieve = context.Students.Where(s => s.Name == "Bob").ToList();

                ViewResult current = (ViewResult)sc.Details(retrieve[0].ID).Result;
                StudentDetailsViewModel test = (StudentDetailsViewModel)current.Model;

                Assert.Equal("Bob", test.Student.Name);
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
                Student student = new Student();
                student.Name = "Bob";
                student.Age = 24;
                student.CourseID = "cs201";

                StudentsController sc = new StudentsController(context);
                var x = sc.Create(student);
                var retrieve = context.Students.Where(s => s.Name == "Bob").ToList();

                ViewResult current = (ViewResult)sc.Details(retrieve[0].ID).Result;
                StudentDetailsViewModel test = (StudentDetailsViewModel)current.Model;

                Assert.Equal("Bob", test.Student.Name);
                student.Name = "Joe";
                x = sc.Update(1, student);

                retrieve = context.Students.Where(s => s.Name == "Joe").ToList();
                ViewResult update = (ViewResult)sc.Details(retrieve[0].ID).Result;
                test = (StudentDetailsViewModel)update.Model;

                Assert.Equal("Joe", test.Student.Name);
            }
        }

        [Fact]
        public void StudentCanBeDelete()
        {
            DbContextOptions<Lab13StudentEnrollmentDbContext> options =
                new DbContextOptionsBuilder<Lab13StudentEnrollmentDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (Lab13StudentEnrollmentDbContext context = new Lab13StudentEnrollmentDbContext(options))
            {
                Student student = new Student();
                student.Name = "Bob";
                student.Age = 24;
                student.CourseID = "cs201";

                StudentsController sc = new StudentsController(context);
                var x = sc.Create(student);
                var retrieve = context.Students.Where(s => s.Name == "Bob").ToList();

                ViewResult current = (ViewResult)sc.Details(retrieve[0].ID).Result;
                StudentDetailsViewModel test = (StudentDetailsViewModel)current.Model;

                Assert.Equal("Bob", test.Student.Name);

                x = sc.Delete(test.Student.ID);
                retrieve = context.Students.Where(s => s.Name == "Bob").ToList();
                Assert.Empty(retrieve);

            }
        }
    }
}
