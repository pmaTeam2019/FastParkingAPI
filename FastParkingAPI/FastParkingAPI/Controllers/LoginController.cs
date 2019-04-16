using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FastParkingAPI.Models;
using FastParkingAPI.Dtos;
using System.Dynamic;
using FastParkingAPI.Constants;
using Swashbuckle.Swagger.Annotations;
using Swashbuckle.Examples;
using FastParkingAPI.Models.Examples.LoginExamples;

namespace FastParkingAPI.Controllers
{
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;

        public LoginController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        /// <summary>
        /// Logs Customers and Owners
        /// </summary>
        /// <param name="login">Object with email and password attributes.</param>
        [HttpPost]
        [SwaggerRequestExample(typeof(Login), typeof(LoginRequestExample))]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(LoginResponseExample))]
        [SwaggerResponse(HttpStatusCode.OK,Type=typeof(LoginResponse))]
        [Route("FastParking/v1/login/authenticate")]
        public IHttpActionResult Authenticate(Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.BadRequest;
                    response.Message = Constants.ErrorMessage.bad_request;
                    return Content(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    var customer = _context.Customers.Where(x => x.Email == login.Email && x.Password == login.Password
                    && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();

                    var owner = _context.Owners.Where(x => x.Email == login.Email && x.Password == login.Password
                    && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();

                    if (customer == null && owner == null)
                    {
                        response.Status = Constants.ResponseStatus.error;
                        response.Code = HttpStatusCode.Unauthorized;
                        response.Message = "Wrong email or password.";
                        return Content(HttpStatusCode.Unauthorized, response);
                    }
                    else
                    {
                        var token = TokenGenerator.GenerateTokenJwt(login.Email);
                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Token = token;
                        return Ok(response);
                    }
                }
            }
            catch (Exception e)
            {
                response.Status = Constants.ResponseStatus.error;
                response.Message = Constants.ErrorMessage.internal_server_error;
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
