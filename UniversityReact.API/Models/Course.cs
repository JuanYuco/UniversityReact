using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityReact.API.Models
{
    public class Course
    {

        public int CourseID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "El titulo debe tener entre 3 y 50 caracteres", MinimumLength = 3)]
        public string Title { get; set; }
        public int ?Credits { get; set; }
    }
}