using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityReact.API.Models
{
    public class Instructor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Debe contener entre 3 y 50 caracteres", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Debe contener entre 3 y 50 caracteres", MinimumLength = 3)]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        public DateTime ?HireDate { get; set; }
    }
}