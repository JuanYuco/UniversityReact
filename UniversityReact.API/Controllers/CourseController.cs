using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UniversityReact.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Course")]
    public class CourseController : ApiController
    {
        private BL.Course course = new BL.Course();

        [HttpGet]
        [Route("GetCourses")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var courses = course.GetCourses();
                return Ok(courses);
            }catch(Exception e)
            {
                return InternalServerError(new Exception("Hubo un error interno por favor contacte al administrador"));
            }
        }
    }
}
