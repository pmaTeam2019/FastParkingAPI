using FastParkingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FastParkingAPI.Dtos;
using AutoMapper;
using System.Dynamic;

namespace FastParkingAPI.Controllers
{
    public class ReservationController : ApiController
    {
        private FastParkingEntities _context;
        private dynamic response;
        private string entityName = "Reservation";

        public ReservationController()
        {
            _context = new FastParkingEntities();
            response = new ExpandoObject();
        }

        [HttpGet]
        [Route("FastParking/v1/customers/{customerId:int}/reservations")]
        public IHttpActionResult getAllByCustomer(int customerId)
        {
            try
            {
                var reservationsModel = _context.Reservations.Where(x => x.ActiveField == Constants.EntityStatus.active && x.CustomerId == customerId).ToList();
                var reservationDto = reservationsModel.Select(Mapper.Map<Reservation, ReservationDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.Reservations = reservationDto;
                response.TotalResults = reservationDto.Count();
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
        [Route("FastParking/v1/owners/{ownerId:int}/reservations")]
        public IHttpActionResult getAllByOwner(int ownerId)
        {
            try
            {
                var reservationsModel = _context.Reservations.Where(x => x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).ToList();
                var reservationDto = reservationsModel.Select(Mapper.Map<Reservation, ReservationDto>);
                response.Status = Constants.ResponseStatus.ok;
                response.Reservations = reservationDto;
                response.TotalResults = reservationDto.Count();
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
        [Route("FastParking/v1/customers/{customerId:int}/reservations/{id:int}")]
        public IHttpActionResult getByIdCustomers(int customerId, int id)
        {
            try
            {
                var reservationModel = _context.Reservations.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.CustomerId == customerId).FirstOrDefault();

                //Resources not found
                if (reservationModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var reservationDto = Mapper.Map<Reservation, ReservationDto>(reservationModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.Reservation = reservationDto;
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
        [Route("FastParking/v1/owners/{ownerId:int}/reservations/{id:int}")]
        public IHttpActionResult getByIdOwners(int ownerId, int id)
        {
            try
            {
                var reservationModel = _context.Reservations.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();

                //Resources not found
                if (reservationModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }

                var reservationDto = Mapper.Map<Reservation, ReservationDto>(reservationModel);
                response.Status = Constants.ResponseStatus.ok;
                response.Code = HttpStatusCode.OK;
                response.Reservation = reservationDto;
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
        [Route("FastParking/v1/customers/{customerId:int}/reservations")]
        public IHttpActionResult Create([FromBody] ReservationDto reservationDto, int customerId)
        {
            Reservation reservationModel;
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
                    reservationDto.CustomerId = customerId;
                    reservationDto.ActiveField = Constants.EntityStatus.active;
                    reservationModel = Mapper.Map<ReservationDto, Reservation>(reservationDto);
                    _context.Reservations.Add(reservationModel);
                    _context.SaveChanges();

                    reservationDto.Id = reservationModel.Id;
                    response.Status = Constants.ResponseStatus.ok;
                    response.Code = HttpStatusCode.Created;
                    response.Reservation = reservationDto;
                    return Created(new Uri($"{Request.RequestUri}/{reservationDto.Id}"), response);
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
        [Route("FastParking/v1/customers/{customerId:int}/reservations/{id:int}")]
        public IHttpActionResult EditByCustomers([FromBody] ReservationDto reservationDto, int customerId, int id)
        {
            Reservation reservationModel;
            try
            {
                reservationModel = _context.Reservations.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.CustomerId == customerId).FirstOrDefault();
                if (reservationModel == null)
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
                        reservationDto.Id = id;
                        reservationDto.CustomerId = customerId;
                        reservationDto.ActiveField = Constants.EntityStatus.active;
                        Mapper.Map(reservationDto, reservationModel);
                        reservationModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        reservationDto = Mapper.Map<Reservation, ReservationDto>(reservationModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Reservation = reservationDto;
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

        [HttpPut]
        [Route("FastParking/v1/owners/{ownerId:int}/reservations/{id:int}")]
        public IHttpActionResult EditByOwner([FromBody] ReservationDto reservationDto, int ownerId, int id)
        {
            Reservation reservationModel;
            try
            {
                reservationModel = _context.Reservations.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.OwnerId == ownerId).FirstOrDefault();
                if (reservationModel == null)
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
                        reservationDto.Id = id;
                        reservationDto.OwnerId = ownerId;
                        reservationDto.ActiveField = Constants.EntityStatus.active;
                        Mapper.Map(reservationDto, reservationModel);
                        reservationModel.ActiveField = Constants.EntityStatus.active;
                        _context.SaveChanges();
                        //set Dto
                        reservationDto = Mapper.Map<Reservation, ReservationDto>(reservationModel);

                        response.Status = Constants.ResponseStatus.ok;
                        response.Code = HttpStatusCode.OK;
                        response.Reservation = reservationDto;
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
        [Route("FastParking/v1/customers/{customersId:int}/reservations/{id:int}")]
        public IHttpActionResult Delete(int customerId, int id)
        {
            Reservation reservationModel;
            try
            {
                reservationModel = _context.Reservations.Where(x => x.Id == id &&
                x.ActiveField == Constants.EntityStatus.active && x.CustomerId == customerId).FirstOrDefault();
                if (reservationModel == null)
                {
                    response.Status = Constants.ResponseStatus.error;
                    response.Code = HttpStatusCode.NotFound;
                    response.Message = string.Format(Constants.ErrorMessage.not_found, entityName, id);
                    return Content(HttpStatusCode.NotFound, response);
                }
                else
                {
                    reservationModel.ActiveField = Constants.EntityStatus.inactive;
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
