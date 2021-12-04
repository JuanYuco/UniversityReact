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
    [RoutePrefix("api/OfficeAssignment")]
    public class OfficeAssignmentController : ApiController
    {
        private BL.OfficeAssignment officeAssignment = new BL.OfficeAssignment();
        private const string INTERNAL_SERVER_ERROR_MSG = "Hubo un error interno por favor contacte al administrador";

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
                return InternalServerError(new Exception(INTERNAL_SERVER_ERROR_MSG));
            }
        }
    }
}
