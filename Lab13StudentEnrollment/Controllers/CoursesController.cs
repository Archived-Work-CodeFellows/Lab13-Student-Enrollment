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
        /// <summary>
        /// Setting up dependency injection for our database
        /// </summary>
        /// <param name="context">Name of the variable we setting</param>
        public CoursesController(Lab13StudentEnrollmentDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Action that is the default landing page and
        /// shows results based on a search entry
        /// </summary>
        /// <param name="search">User inputted string</param>
        /// <returns>Either all or filtered results</returns>
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
        /// <summary>
        /// Action that simply gets the view
        /// </summary>
        /// <returns>View for Create</returns>
        [HttpGet]
        public IActionResult Create() => View();
        /// <summary>
        /// Action that binds the user given info and creates a new course
        /// and adds it to the database
        /// </summary>
        /// <param name="course">New course with bound data</param>
        /// <returns>Redirects to the index</returns>
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID,CourseID,Description")]Course course)
        {
            if (ModelState.IsValid)
            {
                await _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        /// <summary>
        /// Action that will show details of a valid id
        /// </summary>
        /// <param name="id">Expects id or null</param>
        /// <returns>If id is valid, it will return appropriate view, 
        /// else Redirect to index</returns>
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
        /// <summary>
        /// Action that updates the information of a current selected course
        /// </summary>
        /// <param name="id">Id of the current course</param>
        /// <param name="course">New course object with the bound data</param>
        /// <returns>Redirects to index</returns>
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
        /// <summary>
        /// Action that allows user to delete a course. If first checks if there are any
        /// students attached to that course and if it does, it will prevent
        /// a deletion
        /// </summary>
        /// <param name="id">id of the course</param>
        /// <returns>Redirects to Index</returns>
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
