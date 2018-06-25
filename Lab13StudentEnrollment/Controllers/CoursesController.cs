using Lab13StudentEnrollment.Data;
using Lab13StudentEnrollment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab13StudentEnrollment.Controllers
{
    public class CoursesController : Controller
    {
        private Lab13StudentEnrollmentDbContext _context;

        public CoursesController(Lab13StudentEnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            var viewAll = await _context.Courses.ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                var filtered = viewAll.Where(c => c.CourseID.Contains(search)).ToList();
                return View(filtered);
            }
            return View(viewAll);
        }
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID,CourseID,Description")]Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                return View(await CourseDetailsViewModel.ViewDetails(id.Value, _context));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, [Bind("ID,CourseID, Description")]Course course)
        {
            if (ModelState.IsValid)
            {
                var getCourse = _context.Courses.Where(c => c.ID == id).Single();
                var getStudents = _context.Students.Where(s => s.CourseID == getCourse.CourseID);

                foreach (var student in getStudents)
                {
                    Student updatestudent = student;
                    updatestudent.CourseID = course.CourseID;

                    _context.Students.Update(student);
                }
                await _context.SaveChangesAsync();
                _context.Entry(getCourse).State = EntityState.Detached;

                course.ID = id;
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var getCourse = await _context.Courses.Where(c => c.ID == id).SingleAsync();
            var checkStudents = _context.Students.Where(s => s.CourseID == getCourse.CourseID);

            if(!checkStudents.Any())
            {
                _context.Courses.Remove(getCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
