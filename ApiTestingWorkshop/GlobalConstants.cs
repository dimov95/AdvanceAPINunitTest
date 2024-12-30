using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestingWorkshop
{
    public static class GlobalConstants
    {
        public const string BaseUrl = "http://localhost:5000/api";

        public static string AuthenticateUser(string email, string password)
        {
            var resource = email == "admin@gmail.com" ? "user/admin-login" : "user/login";

            var authClient = new RestClient(BaseUrl);
            var restRequest = new RestRequest(resource, Method.Post);
            restRequest.AddJsonBody(new { email, password });
            var response = authClient.Post(restRequest);

            if (response.StatusCode != HttpStatusCode.OK)
            {

                Assert.Fail($"Authentication failed. Status code :{response.StatusCode}, with content: {response.Content}");
            }

                var content = JObject.Parse(response.Content);
            return content["token"]?.ToString();
        }

    }
}
