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
    [RoutePrefix("api/Auth")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private BL.User usuario = new BL.User();

        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(Models.User userModel)
        {
            try
            {
                if ( !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if ( !usuario.FindByEmail( userModel.email ) )
                {
                    return BadRequest("El correo ya esta en uso");
                }

                var result = await usuario.CreateUser(userModel);
                return Ok(result);
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return InternalServerError(e);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> Login ( Models.LoginViewModel model )
        {
            try
            {
                if ( !ModelState.IsValid )
                {
                    return BadRequest(ModelState);
                }

                var loginResult = await usuario.getUserByEmailAndPassword( model.email, model.password );
                if ( loginResult == null )
                {
                    return Unauthorized();
                }

                string token = BL.TokenGenerator.GenerateTokenJwt(model.email);
                return Ok(new Models.View_Models.LoginViewModelReturn
                {
                    id = loginResult.id,
                    name = loginResult.name,
                    lastName = loginResult.lastName,
                    email = loginResult.email,
                    token = token
                });
            } catch ( Exception e )
            {
                Console.WriteLine(e);
                return InternalServerError(new Exception("Ha ocurrido un error interno, por favor contacte con el administrador"));
            }
        }
    }
}
