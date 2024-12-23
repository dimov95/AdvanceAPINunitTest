using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestingWorkshop
{
    internal class GlobalConstants
    {
        public const string BaseUrl = "http://localhost:5000/api";

        public static string AuthenticateUser(string email, string password)
        {
            var resource = email == "admin@gmail.com" ? "user/admin-login" : "user/login";

            var authClient = new RestClient(BaseUrl);
            var restRequest = new RestRequest(resource, Method.Post);
            restRequest.AddJsonBody(new{email, password });
            var response = authClient.Post(restRequest);
            return "";
        }

    }
}
