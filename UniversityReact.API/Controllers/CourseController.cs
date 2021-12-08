using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace UniversityReact.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Course")]
    public class CourseController : ApiController
    {
        private BL.Course course = new BL.Course();
        private const string INTERNAL_SERVER_ERROR_MSG = "Hubo un error interno por favor contacte al administrador";
        private const string BAD_REQUEST_ID = "No existe un curso con el id suministrado";

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var courses = course.GetCourses();
                return Ok(courses);
            }catch(Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(Models.Course courseModel)
        {
            try
            {
                if ( !ModelState.IsValid )
                {
                    return BadRequest( ModelState );
                }

                var courseAdd = await course.CreateCourse(courseModel);
                if ( courseAdd == null )
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok(courseAdd);
            } catch( Exception e)
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
                var result = await course.getCourseById(id);
                if (result == null) return BadRequest(BAD_REQUEST_ID);

                return Ok(result);
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IHttpActionResult> Update(Models.Course courseModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if ( await course.getCourseById(courseModel.CourseID) == null )
                {
                    return BadRequest("No existe un curso con el id suministrado en el modelo");
                }

                var courseUpdate = await course.UpdateCourse(courseModel);
                if ( courseUpdate == null ) InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));

                return Ok(courseUpdate);
            } catch (Exception e)
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
                if ( await course.getCourseById(id) == null )
                {
                    return BadRequest(BAD_REQUEST_ID);
                }

                if ( await course.existsCourseInOtherTable(id) )
                {
                    return BadRequest("El curso que desea eliminar se esta utilizando");
                }

                if ( !course.DeleteCourse(id) )
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok();
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }
    }
}
