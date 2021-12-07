using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityReact.API.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Debe contener entre 3 y 50 caracteres", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public Decimal ?Budget { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ?StartDate { get; set; }

        [Required]
        public int ?InstructorID { get; set; }

        public virtual Instructor Instructor { get; set; }
    }
}