using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace UniversityReact.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/CourseInstructor")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class CourseInstructorController : ApiController
    {
        private BL.CourseInstructor courseInstructor = new BL.CourseInstructor();
        private const string INTERNAL_SERVER_ERROR_MSG = "Hubo un error interno por favor contacte al administrador";
        private const string BAD_REQUEST_ID = "No existe registro con el id suministrado";

        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var departments = await courseInstructor.GetCourseInstructor();
                return Ok(departments);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(Models.CourseInstructor courseInstructorModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if ( await courseInstructor.existsCourseByOtherInstructor( courseInstructorModel.CourseID, null ) )
                {
                    return BadRequest("El curso enviado ya tiene asignado un instructor");
                }

                var courseInstructorAdd = await courseInstructor.CreateCoursesInstructor(courseInstructorModel);
                if (courseInstructorAdd == null)
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok(courseInstructorAdd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var result = await courseInstructor.getCourseInstructorById(id);
                if (result == null) return BadRequest(BAD_REQUEST_ID);

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpGet]
        [Route("GetByCourse")]
        public async Task<IHttpActionResult> GetByCourse(int id)
        {
            try
            {
                var result = await courseInstructor.getCourseInstructorByCourse(id);
                if (result == null) return BadRequest(BAD_REQUEST_ID);

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpGet]
        [Route("GetByInstructor")]
        public async Task<IHttpActionResult> GetByInstructor(int id)
        {
            try
            {
                var result = await courseInstructor.getCourseInstructorByInstructor(id);
                if (result == null) return BadRequest(BAD_REQUEST_ID);

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IHttpActionResult> Update(Models.CourseInstructor courseInstructorModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await courseInstructor.getCourseInstructorById(courseInstructorModel.ID) == null)
                {
                    return BadRequest("No existe un registro con el id suministrado en el modelo");
                }

                if ( await courseInstructor.existsCourseByOtherInstructor( courseInstructorModel.CourseID, courseInstructorModel.ID ) )
                {
                    return BadRequest("El curso suministrado ya tiene un instructor");
                }

                var courseInstructorUpdate = await courseInstructor.UpdateCoursesInstructor(courseInstructorModel);
                if (courseInstructorUpdate == null) InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));

                return Ok(courseInstructorUpdate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (await courseInstructor.getCourseInstructorById(id) == null)
                {
                    return BadRequest(BAD_REQUEST_ID);
                }

                if (!courseInstructor.DeleteCourseInstructor(id))
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }
    }
}
