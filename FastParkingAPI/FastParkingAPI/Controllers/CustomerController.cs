using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FastParkingAPI.Models;
using System.Dynamic;
using FastParkingAPI.Dtos;
using AutoMapper;
using FastParkingAPI.Constants;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using Swashbuckle.Swagger.Annotations;
using Swashbuckle.Examples;
using FastParkingAPI.Models.Examples;
using FastParkingAPI.Models.Examples.CustomerExamples;


namespace FastParkingAPI.Controllers
{
    public class CustomerController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "Customer";

        public CustomerController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        ///<summary>Finds all customers</summary>
        [HttpGet]
        [Authorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<CustomerDto>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Something went wrong.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Authorization has been denied for this request.")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(CustomersExample))]
        [Route("FastParking/v1/customers")]
        public IHttpActionResult getAll()
        {
            try
            {
                var customersModel = _context.Customers.Where(x => x.ActiveField == Constants.EntityStatus.active).ToList();
                var customersDto = customersModel.Select(Mapper.Map<Customer, CustomerDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.Customers = customersDto;
                response.TotalResults = customersDto.Count();
                response.Code = HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Status = Constants.ResponseStatus.error;
                response.Message = Constants.ErrorMessage.internal_server_error;
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }

        ///<summary>Get customer by Id</summary>
        ///<param name="id">Unique customer ID</param>
        [HttpGet]
        [Authorize]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CustomerDto))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,Description = "Something went wrong.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized,Description = "Authorization has been denied for this request.")]
        [SwaggerResponseExample(HttpStatusCode.OK,typeof(CustomerExample))]
        [Route("FastParking/v1/customers/{id:int}")]
        public IHttpActionResult getById(int id)
        {
            try
            {
                var customerModel = _context.Customers.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();

                //Resources not found
                if (customerModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var customerDto = Mapper.Map<Customer, CustomerDto>(customerModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.Customer = customerDto;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Status = Constants.ResponseStatus.error;
                response.Message = Constants.ErrorMessage.internal_server_error;
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Create customer
        /// </summary>
        /// <param name="customerDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CustomerDto))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Something went wrong.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Authorization has been denied for this request.")]
        [SwaggerResponseExample(HttpStatusCode.OK, typeof(CustomerExample))]
        [Route("FastParking/v1/customers")]
        public IHttpActionResult Create([FromBody] CustomerDto customerDto)
        {
            Customer customerModel;
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
                    customerModel = Mapper.Map<CustomerDto, Customer>(customerDto);
                    customerModel.ActiveField = Constants.EntityStatus.active;
                    _context.Customers.Add(customerModel);
                    _context.SaveChanges();

                    customerDto.Id = customerModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.Customer = customerDto;
                    return Created(new Uri($"{Request.RequestUri}/{customerDto.Id}"), response);
                }

            }
            catch (Exception e)
            {
                response.Status = Constants.ResponseStatus.error;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = Constants.ErrorMessage.internal_server_error;
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Updated customer
        /// </summary>
        /// <param name="customerDto">Object with attributes:Id,FirstName,LastName,Addres,FullName.</param>
        /// <param name="id">Unique customer ID</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("FastParking/v1/customers/{id:int}")]
        public IHttpActionResult Edit([FromBody] CustomerDto customerDto, int id)
        {
            Customer customerModel;
            try
            {
                customerModel = _context.Customers.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (customerModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
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
                        customerDto.Id = id;
                        Mapper.Map(customerDto, customerModel);
                        customerModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        customerDto = Mapper.Map<Customer, CustomerDto>(customerModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Customer = customerDto;
                        return Ok(response);
                    }
                }
            }
            catch (Exception e)
            {
                response.Status = Constants.ResponseStatus.error;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = Constants.ErrorMessage.internal_server_error;
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Delete customer
        /// </summary>
        /// <param name="id">Unique customer ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("FastParking/v1/customers/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            Customer customerModel;
            try
            {
                customerModel = _context.Customers.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (customerModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    customerModel.ActiveField = Constants.EntityStatus.inactive;
                    _context.SaveChanges();
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.OK;
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                response.Status = Constants.ResponseStatus.error;
                response.Code = HttpStatusCode.InternalServerError;
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }

    }
}
