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

        public StudentsController(Lab13StudentEnrollmentDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewAll = await _context.Students.ToListAsync();

            return View(viewAll);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(StudentCreateViewModel.CreateStudent(_context));
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("ID,Name,Age,CourseID")]Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

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
