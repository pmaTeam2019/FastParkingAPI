using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swashbuckle.Examples;
using FastParkingAPI.Dtos;

namespace FastParkingAPI.Models.Examples.CustomerExamples
{
    public class CustomerExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CustomerDto
            {
                Id = 1,
                FirstName = "Kevin",
                LastName = "Aranibar Villegas",
                Address = "",
                Birthday = DateTime.Parse("1996-03-23"),
                FullName = "",
                Dni = "79203299",
                Ruc = "20514067881",
                Email = "kevinaranibarvillegas@gmail.com"
            };
        }
    }
}