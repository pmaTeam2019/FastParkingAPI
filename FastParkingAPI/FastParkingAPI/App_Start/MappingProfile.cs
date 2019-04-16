using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using FastParkingAPI.Models;
using FastParkingAPI.Dtos;

namespace FastParkingAPI.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Owner, OwnerDto>().ReverseMap();
            CreateMap<PaymentType, PaymentTypeDto>().ReverseMap();
            CreateMap<CalculationType, CalculationTypeDto>().ReverseMap();
            CreateMap<OwnerPaymentType, OwnerPaymentTypeDto>().ReverseMap();
            CreateMap<OwnerCalculationType, OwnerCalculationTypeDto>().ReverseMap();
            CreateMap<Slot, SlotDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            
        }
    }
}