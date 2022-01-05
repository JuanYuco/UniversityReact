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
    [RoutePrefix("api/OfficeAssignment")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class OfficeAssignmentController : ApiController
    {
        private BL.OfficeAssignment officeAssignment = new BL.OfficeAssignment();
        private const string INTERNAL_SERVER_ERROR_MSG = "Hubo un error interno por favor contacte al administrador";
        private const string BAD_REQUEST_ID = "No existe un officina asignada con el id de instructor suministrado suministrado";

        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var officesAssignment = await officeAssignment.GetOfficesAssignment();
                return Ok(officesAssignment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(Models.OfficeAssignment officeAssignmentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if ( await officeAssignment.getOfficeAssignmentById( officeAssignmentModel.InstructorID ) != null )
                {
                    return BadRequest("El Instructor ya tiene asignada una oficina");
                }

                var officeAssignmentAdd = await officeAssignment.CreateOfficeAssignment(officeAssignmentModel);
                if (officeAssignmentAdd == null)
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok(officeAssignmentAdd);
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
                var result = await officeAssignment.getOfficeAssignmentById(id);
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
        public async Task<IHttpActionResult> Update(Models.OfficeAssignment officeAssignmentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await officeAssignment.getOfficeAssignmentById(officeAssignmentModel.InstructorID) == null)
                {
                    return BadRequest("No existe un curso con el id suministrado en el modelo");
                }

                var officeAssignmentUpdate = await officeAssignment.UpdateOfficeAssignment(officeAssignmentModel);
                if (officeAssignmentUpdate == null) InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));

                return Ok(officeAssignmentUpdate);
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
                if (await officeAssignment.getOfficeAssignmentById(id) == null)
                {
                    return BadRequest(BAD_REQUEST_ID);
                }

                if (!officeAssignment.DeleteOfficeAssignment(id))
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
