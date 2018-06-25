using Lab13StudentEnrollment.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab13StudentEnrollment.Models
{
    public class StudentDetailsViewModel
    {
        public IEnumerable<SelectListItem> Courses { get; set; }
        public Student Student { get; set; }

        public static async Task<StudentDetailsViewModel> ViewDetails(int id, Lab13StudentEnrollmentDbContext context)
        {
            StudentDetailsViewModel sdvm = new StudentDetailsViewModel();

            sdvm.Student = await context.Students.Where(s => s.ID == id).SingleAsync();
            sdvm.Courses = new SelectList(
                context.Courses.Select(c => c.CourseID)

            );

            return sdvm;
        }
    }
}
