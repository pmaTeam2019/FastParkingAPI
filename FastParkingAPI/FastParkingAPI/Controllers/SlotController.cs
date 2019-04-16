using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FastParkingAPI.Dtos;
using FastParkingAPI.Models;

namespace FastParkingAPI.Controllers
{
    public class SlotController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "Slot";

        public SlotController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        [HttpGet]
        [Route("FastParking/v1/owners/{ownerId:int}/slots")]
        public IHttpActionResult getAll(int ownerId)
        {
            try
            {
                var slotsModel = _context.Slots.Where(x => x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).ToList();
                var slotsDto = slotsModel.Select(Mapper.Map<Slot, SlotDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.Slots = slotsDto;
                response.TotalResults = slotsDto.Count();
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
        [Route("FastParking/v1/owners/{ownerId:int}/slots/{id:int}")]
        public IHttpActionResult getById(int ownerId, int id)
        {
            try
            {
                var slotModel = _context.Slots.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();

                //Resources not found
                if (slotModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var slotDto = Mapper.Map<Slot,SlotDto>(slotModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.Slot = slotDto;
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
        [Route("FastParking/v1/owners/{ownerId:int}/slots")]
        public IHttpActionResult Create([FromBody] SlotDto slotDto, int ownerId)
        {
            Slot slotModel;
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
                    slotDto.OwnerId = ownerId;
                    slotDto.ActiveField = Constants.EntityStatus.active;
                    slotModel = Mapper.Map<SlotDto, Slot>(slotDto);
                    _context.Slots.Add(slotModel);
                    _context.SaveChanges();

                    slotDto.Id = slotModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.Slot = slotDto;
                    return Created(new Uri($"{Request.RequestUri}/{slotDto.Id}"), response);
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
        [Route("FastParking/v1/owners/{ownerId:int}/slots/{id:int}")]
        public IHttpActionResult Edit([FromBody] SlotDto slotDto, int ownerId, int id)
        {
            Slot slotModel;
            try
            {
                slotModel = _context.Slots.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();
                if (slotModel == null)
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
                        slotDto.Id = id;
                        slotDto.OwnerId = ownerId;
                        slotDto.ActiveField = Constants.EntityStatus.active;
                        Mapper.Map(slotDto, slotModel);
                        slotModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        slotDto = Mapper.Map<Slot, SlotDto>(slotModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Slot = slotDto;
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
        [Route("FastParking/v1/owners/{ownerId:int}/slots/{id:int}")]
        public IHttpActionResult Delete(int ownerId, int id)
        {
            Slot slotModel;
            try
            {
                slotModel = _context.Slots.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();
                if (slotModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    slotModel.ActiveField = Constants.EntityStatus.inactive;
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
