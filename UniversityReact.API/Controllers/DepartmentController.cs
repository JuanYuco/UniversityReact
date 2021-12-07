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
    [RoutePrefix("api/Department")]
    public class DepartmentController : ApiController
    {
        private BL.Department department = new BL.Department();
        private const string INTERNAL_SERVER_ERROR_MSG = "Hubo un error interno por favor contacte al administrador";
        private const string BAD_REQUEST_ID = "No existe un departamento con el id suministrado";

        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var departments = await department.GetDepartments();
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
        public async Task<IHttpActionResult> Create(Models.Department departmentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var departmentAdd = await department.CreateDepartment(departmentModel);
                if (departmentAdd == null)
                {
                    return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
                }

                return Ok(departmentAdd);
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
                var result = await department.getDepartmentById(id);
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
        public async Task<IHttpActionResult> Update(Models.Department departmentModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await department.getDepartmentById(departmentModel.DepartmentID) == null)
                {
                    return BadRequest("No existe un departamento con el id suministrado en el modelo");
                }

                var departmentUpdate = await department.UpdateDepartment(departmentModel);
                if (departmentUpdate == null) InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));

                return Ok(departmentUpdate);
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
                if (await department.getDepartmentById(id) == null)
                {
                    return BadRequest(BAD_REQUEST_ID);
                }

                if (!department.DeleteDepartment(id))
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
