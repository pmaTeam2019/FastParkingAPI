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
using FastParkingAPI.Providers;

namespace FastParkingAPI.Controllers
{
    public class OwnerController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "Owner";

        public OwnerController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        ///<summary>
        ///This will list all Owners
        ///</summary>
        [HttpGet]
        [Route("FastParking/v1/owners")]
        public IHttpActionResult getAll()
        {
            try
            {
                var ownersModel = _context.Owners.Where(x => x.ActiveField == Constants.EntityStatus.active).ToList();
                var ownersDto = ownersModel.Select(Mapper.Map<Owner, OwnerDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.Owners = ownersDto;
                response.TotalResults = ownersDto.Count();
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
        [Route("FastParking/v1/owners/{id:int}")]
        public IHttpActionResult getById(int id)
        {
            try
            {
                var ownerModel = _context.Owners.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();

                //Resources not found
                if (ownerModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var ownerDto = Mapper.Map<Owner, OwnerDto>(ownerModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.Owner = ownerDto;
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
        [Route("FastParking/v1/owners")]
        public IHttpActionResult Create([FromBody] OwnerDto ownerDto)
        {
            Owner ownerModel;
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
                    ownerModel = Mapper.Map<OwnerDto, Owner>(ownerDto);
                    ownerModel.ActiveField = Constants.EntityStatus.active;
                    _context.Owners.Add(ownerModel);
                    _context.SaveChanges();

                    ownerDto.Id = ownerModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.Owner = ownerDto;
                    return Created(new Uri($"{Request.RequestUri}/{ownerDto.Id}"), response);
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

        ///<summary></summary>
        [HttpPut]
        [Route("FastParking/v1/owners/{id:int}")]
        public IHttpActionResult Edit([FromBody] OwnerDto ownerDto, int id)
        {
            Owner ownerModel;
            try
            {
                ownerModel = _context.Owners.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (ownerModel == null)
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
                        ownerDto.Id = id;
                        Mapper.Map(ownerDto, ownerModel);
                        ownerModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        ownerDto = Mapper.Map<Owner, OwnerDto>(ownerModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Customer = ownerDto;
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

        ///<summary></summary>
        [HttpDelete]
        [Route("FastParking/v1/owners/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            Owner ownerModel;
            try
            {
                ownerModel = _context.Owners.Where(x => x.Id == id && x.ActiveField == Constants.EntityStatus.active).FirstOrDefault();
                if (ownerModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    ownerModel.ActiveField = Constants.EntityStatus.inactive;
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
