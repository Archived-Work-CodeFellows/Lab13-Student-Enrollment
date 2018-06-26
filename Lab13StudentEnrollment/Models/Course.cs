using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab13StudentEnrollment.Models
{
    public class Course
    {
        public int ID { get; set; }
        [Required]
        public string CourseID { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
