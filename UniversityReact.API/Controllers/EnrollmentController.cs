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
    [RoutePrefix("api/Enrollment")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class EnrollmentController : ApiController
    {
        private BL.Enrollment enrollment = new BL.Enrollment();
        private const string INTERNAL_SERVER_ERROR_MSG = "Hubo un error interno por favor contacte al administrador";
        private const string BAD_REQUEST_ID = "No existe registro con el id suministrado";

        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var enrollments = await enrollment.GetEnrollments();
                return Ok(enrollments);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(Models.Enrollment enrollmentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await enrollment.existsEnrollment(enrollmentModel.CourseID, enrollmentModel.StudentID, null) )
                {
                    return BadRequest("El curso enviado ya tiene asignado un instructor");
                }

                var enrollmentAdd = await enrollment.CreateEnrollment(enrollmentModel);
                if (enrollmentAdd == null)
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok(enrollmentAdd);
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
                var result = await enrollment.getEnrollmentById(id);
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
                var result = await enrollment.getEnrollmentByCourse(id);
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
        [Route("GetByStudent")]
        public async Task<IHttpActionResult> GetByStudent(int id)
        {
            try
            {
                var result = await enrollment.getEnrollmentByStudent(id);
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
        public async Task<IHttpActionResult> Update(Models.Enrollment enrollmentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await enrollment.getEnrollmentById(enrollmentModel.EnrollmentID) == null)
                {
                    return BadRequest("No existe un registro con el id suministrado en el modelo");
                }

                if (await enrollment.existsEnrollment(enrollmentModel.CourseID, enrollmentModel.StudentID, enrollmentModel.EnrollmentID))
                {
                    return BadRequest("Ya existe otro registro con la misma información");
                }

                var enrollmentUpdate = await enrollment.UpdateEnrollment(enrollmentModel);
                if (enrollmentUpdate == null) InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));

                return Ok(enrollmentUpdate);
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
                if (await enrollment.getEnrollmentById(id) == null)
                {
                    return BadRequest(BAD_REQUEST_ID);
                }

                if (!enrollment.DeleteEnrollment(id))
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
