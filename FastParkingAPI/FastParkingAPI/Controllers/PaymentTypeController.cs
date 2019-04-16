using FastParkingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FastParkingAPI.Dtos;
using System.Dynamic;
using AutoMapper;

namespace FastParkingAPI.Controllers
{
    public class PaymentTypeController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "PaymentType";

        public PaymentTypeController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        ///<summary>
        ///This will list all Customers
        ///</summary>
        [HttpGet]
        [Route("FastParking/v1/paymenttypes")]
        public IHttpActionResult getAll()
        {
            try
            {
                var paymentTypesModel = _context.PaymentTypes.Where(x => x.ActiveField == Constants.EntityStatus.active).ToList();
                var paymentTypesDto = paymentTypesModel.Select(Mapper.Map<PaymentType, PaymentTypeDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.PaymentTypes = paymentTypesDto;
                response.TotalResults = paymentTypesDto.Count();
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

        ///<summary></summary>
        [HttpGet]
        [Route("FastParking/v1/paymenttypes/{id:int}")]
        public IHttpActionResult getById(int id)
        {
            try
            {
                var paymentTypeModel = _context.PaymentTypes.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();

                //Resources not found
                if (paymentTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var paymentTypeDto = Mapper.Map<PaymentType, PaymentTypeDto>(paymentTypeModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.PaymentType = paymentTypeDto;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Status = Constants.ResponseStatus.error;
                response.Message = Constants.ErrorMessage.internal_server_error;
                return Content(HttpStatusCode.InternalServerError, response);
            }
        }

        ///<summary></summary>
        [HttpPost]
        [Route("FastParking/v1/paymenttypes")]
        public IHttpActionResult Create([FromBody] PaymentTypeDto paymentTypeDto)
        {
            PaymentType paymentTypeModel;
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
                    paymentTypeModel = Mapper.Map<PaymentTypeDto, PaymentType>(paymentTypeDto);
                    paymentTypeModel.ActiveField = Constants.EntityStatus.active;
                    _context.PaymentTypes.Add(paymentTypeModel);
                    _context.SaveChanges();

                    paymentTypeDto.Id = paymentTypeModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.PaymentType = paymentTypeDto;
                    return Created(new Uri($"{Request.RequestUri}/{paymentTypeDto.Id}"), response);
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

        [HttpPut]
        [Route("FastParking/v1/paymenttypes/{id:int}")]
        public IHttpActionResult Edit([FromBody] PaymentTypeDto paymentTypeDto, int id)
        {
            PaymentType paymentTypeModel;
            try
            {
                paymentTypeModel = _context.PaymentTypes.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (paymentTypeModel == null)
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
                        paymentTypeDto.Id = id;
                        Mapper.Map(paymentTypeDto, paymentTypeModel);
                        paymentTypeModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        paymentTypeDto = Mapper.Map<PaymentType, PaymentTypeDto>(paymentTypeModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Customer = paymentTypeDto;
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

        [HttpDelete]
        [Route("FastParking/v1/paymenttypes/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            PaymentType paymentTypeModel;
            try
            {
                paymentTypeModel = _context.PaymentTypes.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (paymentTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    paymentTypeModel.ActiveField = Constants.EntityStatus.inactive;
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
