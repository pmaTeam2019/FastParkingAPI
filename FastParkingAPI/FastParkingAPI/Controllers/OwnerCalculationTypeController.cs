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
    public class OwnerCalculationTypeController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "OwnerCalculationType";

        public OwnerCalculationTypeController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }


        [HttpGet]
        [Route("FastParking/v1/owners/{ownerId:int}/ownercalculationtypes")]
        public IHttpActionResult getAll(int ownerId)
        {
            try
            {
                var ownerCalculationTypesModel = _context.OwnerCalculationTypes.Where(x => x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).ToList();
                var ownerCalculationTypesDto = ownerCalculationTypesModel.Select(Mapper.Map<OwnerCalculationType, OwnerCalculationTypeDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.OwnerCalculationTypes = ownerCalculationTypesDto;
                response.TotalResults = ownerCalculationTypesDto.Count();
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownercalculationtypes/{id:int}")]
        public IHttpActionResult getById(int ownerId, int id)
        {
            try
            {
                var ownerCalculationTypeModel = _context.OwnerCalculationTypes.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();

                //Resources not found
                if (ownerCalculationTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var ownerCalculationTypeDto = Mapper.Map<OwnerCalculationType,OwnerCalculationTypeDto>(ownerCalculationTypeModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.OwnerCalculationType = ownerCalculationTypeDto;
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownercalculationtypes")]
        public IHttpActionResult Create([FromBody] OwnerCalculationTypeDto ownerCalculationTypeDto, int ownerId)
        {
            OwnerCalculationType ownerCalculationTypeModel;
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
                    ownerCalculationTypeDto.OwnerId = ownerId;
                    ownerCalculationTypeDto.ActiveField = Constants.EntityStatus.active;
                    ownerCalculationTypeModel = Mapper.Map<OwnerCalculationTypeDto, OwnerCalculationType>(ownerCalculationTypeDto);
                    _context.OwnerCalculationTypes.Add(ownerCalculationTypeModel);
                    _context.SaveChanges();

                    ownerCalculationTypeDto.Id = ownerCalculationTypeModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.OwnerPaymentType = ownerCalculationTypeDto;
                    return Created(new Uri($"{Request.RequestUri}/{ownerCalculationTypeDto.Id}"), response);
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownercalculationtypes/{id:int}")]
        public IHttpActionResult Edit([FromBody] OwnerCalculationTypeDto ownerCalculationTypeDto, int ownerId, int id)
        {
            OwnerCalculationType ownerCalculationTypeModel;
            try
            {
                ownerCalculationTypeModel = _context.OwnerCalculationTypes.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();
                if (ownerCalculationTypeModel == null)
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
                        ownerCalculationTypeDto.Id = id;
                        ownerCalculationTypeDto.OwnerId = ownerId;
                        ownerCalculationTypeDto.ActiveField = Constants.EntityStatus.active;
                        Mapper.Map(ownerCalculationTypeDto, ownerCalculationTypeModel);
                        _context.SaveChanges();
                        //set Dto
                        ownerCalculationTypeDto = Mapper.Map<OwnerCalculationType, OwnerCalculationTypeDto>(ownerCalculationTypeModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.OwnerCalculationType = ownerCalculationTypeDto;
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownercalculationtypes/{id:int}")]
        public IHttpActionResult Delete(int ownerId, int id)
        {
            OwnerCalculationType ownerCalculationTypeModel;
            try
            {
                ownerCalculationTypeModel = _context.OwnerCalculationTypes.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();
                if (ownerCalculationTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    ownerCalculationTypeModel.ActiveField = Constants.EntityStatus.inactive;
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
