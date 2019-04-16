using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swashbuckle.Examples;
using FastParkingAPI.Dtos;

namespace FastParkingAPI.Models.Examples.CustomerExamples
{
    public class CustomersExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<CustomerDto>
           {
               new CustomerDto { Id=1,FirstName="Kevin",LastName="Aranibar Villegas",Address=""
               ,Birthday=DateTime.Parse("1996-03-23"),FullName="",Dni="79203299",Ruc="20514067881",Email="kevinaranibarvillegas@gmail.com"},
                new CustomerDto { Id=2,FirstName="Jose Martin",LastName="Veliz Francia",Address="De San, Av, Los Tusilagos Oeste Nro. 281, San Juan de Lurigancho 15401"
               ,Birthday=DateTime.Parse("1996-03-10"),FullName="",Dni="78493254",Ruc="",Email="jveliz123456"}
           };
        }
    }
}