using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UniversityReact.API.Models
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [StringLength(255)]
        public string email { get; set; }

        [Required]
        [StringLength(45, ErrorMessage = "La contraseña debe contener mínimo 5 caracteres y máximo 45", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}