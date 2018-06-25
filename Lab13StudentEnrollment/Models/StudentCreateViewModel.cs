using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab13StudentEnrollment.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab13StudentEnrollment.Models
{
    public class StudentCreateViewModel
    {
        public IEnumerable<SelectListItem> Courses { get; set; }
        public Student Student { get; set; }

        public static StudentCreateViewModel CreateStudent(Lab13StudentEnrollmentDbContext context)
        {
            StudentCreateViewModel scvm = new StudentCreateViewModel();
            scvm.Courses = new SelectList(
                context.Courses.Select(c => c.CourseID)
                               
            );
            return scvm;
        }
    }
}
