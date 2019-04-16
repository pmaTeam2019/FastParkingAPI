using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FastParkingAPI.Constants
{
    public class ApiInformation
    {
        public string version = "v1";
    }
    public class EntityStatus
    {
        public static bool active = true;
        public static bool inactive = false;
    }
    public class ResponseStatus
    {
        public static string ok = "ok";
        public static string error = "error";
    }
    public class ErrorMessage
    {
        public static string internal_server_error = "Something went wrong";
        public static string bad_request = "Body parameters are not valid";
        public static string not_found = "Entity {0} with Id {1} not found";
    }
}