using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityReact.API.Models.View_Models
{
    public class LoginViewModelReturn
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}