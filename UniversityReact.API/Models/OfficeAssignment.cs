using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityReact.API.Models
{
    public class OfficeAssignment
    {
        [Required]
        [Key]
        public int InstructorID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Debe contener entre 3 y 50 caracteres", MinimumLength = 3)]
        public string Location { get; set; }

        public virtual Instructor Instructor { get; set; }
    }
}