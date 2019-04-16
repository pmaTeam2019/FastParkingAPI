using FastParkingAPI.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FastParkingAPI.Dtos;

namespace FastParkingAPI.Controllers
{
    public class CalculationTypeController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "CalculationType";

        public CalculationTypeController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        ///<summary></summary>
        [HttpGet]
        [Route("FastParking/v1/calculationtypes")]
        public IHttpActionResult getAll()
        {
            try
            {
                var calculationTypesModel = _context.CalculationTypes.Where(x => x.ActiveField == Constants.EntityStatus.active).ToList();
                var calculationTypesDto = calculationTypesModel.Select(Mapper.Map<CalculationType, CalculationTypeDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.CalculationTypes = calculationTypesDto;
                response.TotalResults = calculationTypesDto.Count();
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

        [HttpGet]
        ///<summary></summary>
        [Route("FastParking/v1/calculationtypes/{id:int}")]
        public IHttpActionResult getById(int id)
        {
            try
            {
                var calculationTypeModel = _context.Customers.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();

                //Resources not found
                if (calculationTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var calculationTypeDto = Mapper.Map<Customer, CustomerDto>(calculationTypeModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.CalculationType = calculationTypeDto;
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
        [Route("FastParking/v1/calculationtypes")]
        public IHttpActionResult Create([FromBody] CalculationTypeDto calculationTypeDto)
        {
            CalculationType calculationTypeModel;
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
                    calculationTypeModel = Mapper.Map<CalculationTypeDto, CalculationType>(calculationTypeDto);
                    calculationTypeModel.ActiveField = Constants.EntityStatus.active;
                    _context.CalculationTypes.Add(calculationTypeModel);
                    _context.SaveChanges();

                    calculationTypeDto.Id = calculationTypeModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.Customer = calculationTypeDto;
                    return Created(new Uri($"{Request.RequestUri}/{calculationTypeDto.Id}"), response);
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
        [Route("FastParking/v1/calculationtypes/{id:int}")]
        public IHttpActionResult Edit([FromBody] CalculationTypeDto calculationTypeDto, int id)
        {
            CalculationType calculationTypeModel;
            try
            {
                calculationTypeModel = _context.CalculationTypes.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (calculationTypeModel == null)
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
                        calculationTypeDto.Id = id;
                        Mapper.Map(calculationTypeDto, calculationTypeModel);
                        calculationTypeModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        calculationTypeDto = Mapper.Map<CalculationType, CalculationTypeDto>(calculationTypeModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Customer = calculationTypeDto;
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
        [Route("FastParking/v1/calculationtypes/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            CalculationType calculationTypeModel;
            try
            {
                calculationTypeModel = _context.CalculationTypes.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (calculationTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    calculationTypeModel.ActiveField = Constants.EntityStatus.inactive;
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
