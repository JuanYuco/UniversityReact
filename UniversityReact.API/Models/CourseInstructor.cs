using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityReact.API.Models
{
    public class CourseInstructor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int CourseID { get; set; }

        [Required]
        public int InstructorID { get; set; }

        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}