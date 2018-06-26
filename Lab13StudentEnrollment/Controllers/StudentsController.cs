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
    public class StudentsController : Controller
    {
        private Lab13StudentEnrollmentDbContext _context;

        /// <summary>
        /// Here we are setting up our dependency injection
        /// </summary>
        /// <param name="context">variable we will be using for our database access</param>
        public StudentsController(Lab13StudentEnrollmentDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// This action is the default view greeted to the user.
        /// It take in a string and if that string not null or empty,
        /// it will filter out results from the database
        /// </summary>
        /// <param name="search">Search word entered</param>
        /// <returns>object based on input string/returns>
        public async Task<IActionResult> Index(string search)
        {
            var viewAll = await _context.Students.ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                var filtered = viewAll.Where(s => s.Name.Contains(search)).ToList();
                return View(filtered);
            }
            return View(viewAll);
        }
        /// <summary>
        /// Action that uses a view model to help get the appropriate
        /// information for creating a student
        /// </summary>
        /// <returns>Object that includes information from both Students and Courses Tables</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(StudentCreateViewModel.CreateStudent(_context));
        }
        /// <summary>
        /// Action that posts the given information by users to create a new student
        /// and update the database
        /// </summary>
        /// <param name="student">Binding the info from the form to an instance of student</param>
        /// <returns>Redirects to the Index action</returns>
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID,Name,Age,CourseID")]Student student)
        {
            if (ModelState.IsValid)
            {
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(StudentCreateViewModel.CreateStudent(_context));
        }
        /// <summary>
        /// Action that allows the user to view student specific details.
        /// </summary>
        /// <param name="id">Can accept an id or nothing</param>
        /// <returns>Returns a SDVM base on the object id or redirects to index</returns>
        public async  Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                return View(await StudentDetailsViewModel.ViewDetails(id.Value, _context));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Action is used to update. If first checks if the model state is valid and
        /// then appropriately updates the new given information to the database
        /// </summary>
        /// <param name="id">Id of the current student</param>
        /// <param name="student">Binding the info to an instance of student</param>
        /// <returns>Redirects to Index after completion</returns>
        [HttpPost]
        public async Task<IActionResult> Update(int id,[Bind("ID,Name,Age,CourseID")]Student student)
        {

            if (ModelState.IsValid)
            {
                student.ID = id;

                _context.Students.Update(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Action used to remove a student from the database
        /// </summary>
        /// <param name="id">id of the student in the database</param>
        /// <returns>Returns not found if the student doesn't exist or redirects to index</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if(student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
