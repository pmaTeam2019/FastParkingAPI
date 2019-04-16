using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using Swashbuckle.Examples;

namespace FastParkingAPI.Models.Examples.LoginExamples
{
    public class LoginResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new LoginResponse
            {
                Status = "Ok",
                Code = "202",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImtldmluYXJhbmliYXJ2aWxsZWdhc0BnbWFpbC5jb20iLCJuYmYiOjE1NTU0MjEyOTEsImV4cCI6MTU1NTQyMzA5MSwiaWF0IjoxNTU1NDIxMjkxLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjU0OTcyIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1NDk3MiJ9.J_iUde3BQw7X8IqHS7szk3I8FRAx9VdMmzcGfjoQfXE"
            };
        }
    }
}