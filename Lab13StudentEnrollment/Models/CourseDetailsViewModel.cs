using Lab13StudentEnrollment.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab13StudentEnrollment.Models
{
    public class CourseDetailsViewModel
    {
        public List<string> Students { get; set; }
        public Course Course { get; set; }

        public static async Task<CourseDetailsViewModel> ViewDetails(int id, Lab13StudentEnrollmentDbContext context)
        {
            CourseDetailsViewModel cdvm = new CourseDetailsViewModel();

            cdvm.Course = await context.Courses.Where(c => c.ID == id).SingleAsync();
            cdvm.Students =
                context.Students.Where(s => s.CourseID == cdvm.Course.CourseID)
                                .Select(s => s.Name).ToList();
        
            return cdvm;
        }
    }
}
