using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastParkingAPI.Models.Examples.LoginExamples
{
    public class LoginResponse
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Token { get; set; }
    }
}