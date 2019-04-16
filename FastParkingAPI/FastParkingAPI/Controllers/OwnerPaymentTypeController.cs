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
    public class OwnerPaymentTypeController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "OwnerPaymentType";

        OwnerPaymentTypeController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        [HttpGet]
        [Route("FastParking/v1/owners/{ownerId:int}/ownerpaymenttypes")]
        public IHttpActionResult getAll(int ownerId)
        {
            try
            {
                var ownerPaymentTypesModel = _context.OwnerPaymentTypes.Where(x => x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).ToList();
                var ownerPaymentTypesDto = ownerPaymentTypesModel.Select(Mapper.Map<OwnerPaymentType, OwnerPaymentTypeDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.OwnerPaymentTypes = ownerPaymentTypesDto;
                response.TotalResults = ownerPaymentTypesDto.Count();
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownerpaymenttypes/{id:int}")]
        public IHttpActionResult getById(int ownerId,int id)
        {
            try
            {
                var ownerPaymentTypeModel = _context.OwnerPaymentTypes.Where(x => x.Id == id && 
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();

                //Resources not found
                if (ownerPaymentTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var ownerPaymentTypeDto = Mapper.Map<OwnerPaymentType, OwnerPaymentTypeDto>(ownerPaymentTypeModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.OwnerPaymentType = ownerPaymentTypeDto;
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownerpaymenttypes")]
        public IHttpActionResult Create([FromBody] OwnerPaymentTypeDto ownerPaymentTypeDto, int ownerId)
        {
            OwnerPaymentType ownerPaymentTypeModel;
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
                    ownerPaymentTypeDto.OwnerId = ownerId;
                    ownerPaymentTypeDto.ActiveField = Constants.EntityStatus.active;
                    ownerPaymentTypeModel = Mapper.Map<OwnerPaymentTypeDto, OwnerPaymentType>(ownerPaymentTypeDto);
                    _context.OwnerPaymentTypes.Add(ownerPaymentTypeModel);
                    _context.SaveChanges();

                    ownerPaymentTypeDto.Id = ownerPaymentTypeModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.OwnerPaymentType = ownerPaymentTypeDto;
                    return Created(new Uri($"{Request.RequestUri}/{ownerPaymentTypeDto.Id}"), response);
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownerpaymenttypes/{id:int}")]
        public IHttpActionResult Edit([FromBody] OwnerPaymentTypeDto ownerPaymentTypeDto,int ownerId,int id)
        {
            OwnerPaymentType ownerPaymentTypeModel;
            try
            {
                ownerPaymentTypeModel = _context.OwnerPaymentTypes.Where(x => x.Id == id && 
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();
                if (ownerPaymentTypeModel == null)
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
                        ownerPaymentTypeDto.Id = id;
                        ownerPaymentTypeDto.OwnerId = ownerId;
                        Mapper.Map(ownerPaymentTypeDto, ownerPaymentTypeModel);
                        ownerPaymentTypeModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        ownerPaymentTypeDto = Mapper.Map<OwnerPaymentType, OwnerPaymentTypeDto>(ownerPaymentTypeModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.OwnerPaymentType = ownerPaymentTypeDto;
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
        [Route("FastParking/v1/owners/{ownerId:int}/ownerpaymenttypes/{id:int}")]
        public IHttpActionResult Delete(int ownerId,int id)
        {
            OwnerPaymentType ownerPaymentTypeModel;
            try
            {
                ownerPaymentTypeModel = _context.OwnerPaymentTypes.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();
                if (ownerPaymentTypeModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    ownerPaymentTypeModel.ActiveField = Constants.EntityStatus.inactive;
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
