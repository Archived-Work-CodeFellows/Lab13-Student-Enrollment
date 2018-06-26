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
            Assert.Equal("Joe", student.Name);
        }
    }

}
