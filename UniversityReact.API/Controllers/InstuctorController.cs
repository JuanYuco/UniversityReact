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
    [RoutePrefix("api/Instructor")]
    public class InstuctorController : ApiController
    {
        private BL.Instructor instructor = new BL.Instructor();
        private const string INTERNAL_SERVER_ERROR_MSG = "Hubo un error interno por favor contacte al administrador";
        private const string BAD_REQUEST_ID = "No existe un instructor con el id suministrado";

        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var instructos = instructor.GetInstrutors();
                return Ok(instructos);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> Create(Models.Instructor instructorModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isntructorAdd = await instructor.CreateInstructor(instructorModel);
                if (isntructorAdd == null)
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok(isntructorAdd);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            try
            {
                var result = await instructor.getIntructorById(id);
                if (result == null) return BadRequest(BAD_REQUEST_ID);

                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IHttpActionResult> Update(Models.Instructor instructorModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await instructor.getIntructorById(instructorModel.ID) == null)
                {
                    return BadRequest("No existe un instructor con el id suministrado en el modelo");
                }

                var instructorUpdate = await instructor.UpdateInstructor(instructorModel);
                if (instructorUpdate == null) InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));

                return Ok(instructorUpdate);
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (await instructor.getIntructorById(id) == null)
                {
                    return BadRequest(BAD_REQUEST_ID);
                }

                if (!instructor.DeleteInstructor(id))
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }
    }
}
