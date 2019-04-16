using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swashbuckle.Examples;
using FastParkingAPI.Models;
using FastParkingAPI.Dtos;

namespace FastParkingAPI.Models.Examples.LoginExamples
{
    public class LoginRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new Login
            {
                Email = "kevinaranibarvillegas@gmail.com",
                Password = "u201512321"
            };
        }
    }
}