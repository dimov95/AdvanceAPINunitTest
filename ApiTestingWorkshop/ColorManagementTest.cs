using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestingWorkshop
{
    [TestFixture]
    public class ColorManagementTest
    {
        private RestSharp.RestClient restClient;
        private string token;
        private Random random;

        [TearDown]
        public void Dispose()
        {
            //Delete memory form RestClient
            restClient.Dispose();
        }
        [SetUp]
        public void Setup()
        {
            //Setup the dependencies
            restClient = new RestSharp.RestClient(GlobalConstants.BaseUrl);
            token = GlobalConstants.AuthenticateUser("admin@gmail.com", "admin@gmail.com");
            random = new Random();
        }
        [Test]
        public void ColorLifecycleTest()
        {
            //Make post request with a random title
            var addColorRequest = new RestRequest("/color", Method.Post);
            addColorRequest.AddHeader("Authorization", $"Bearer {token}");
            addColorRequest.AddJsonBody(new
            {
                title = $"Color_{random.Next(999, 9999)}"
            });

            var addColorResponse = restClient.Execute(addColorRequest);

            //Assert that is ready
            Assert.That(addColorResponse.IsSuccessful, Is.True, "The request color failed");

            //Extract color id and make get request by id
            var colorId = JObject.Parse(addColorResponse.Content)["_id"]?.ToString();
            Assert.That(colorId, Is.Not.Null.Or.Empty);

            //Ger all  colors  with a same ID
            var getColorRequest = new RestRequest($"/color/{colorId}", Method.Get);
            var getColorResponse = restClient.Execute(getColorRequest);
            //Assert that the response is successful
            Assert.That(addColorResponse.IsSuccessful, Is.True, "The request color failed");


            //delete color
            var deleteColorRequest = new RestRequest($"/color/{colorId}", Method.Delete);
            deleteColorRequest.AddHeader("Authorization", $"Bearer {token}");
            var deleteResponse = restClient.Execute(deleteColorRequest);
            Assert.That(deleteResponse.IsSuccessful, Is.True, "The request color failed");

            //make a ger request to validate the color is deleted
            var verifyDelete = new RestRequest($"/color/{colorId}", Method.Get);
            //Sent the request to back-end
            var verifyResponse = restClient.Execute(verifyDelete);

            //Assert that  is deleted successfully
            Assert.That(verifyResponse.Content, Is.Null.Or.EqualTo("null"));
        }
        [Test]
        public void ColorLifecycleNegativeTest()
        {
            //Make post request with invalid token
            var invalidToken = "Invalid token";
            var addColorRequest = new RestRequest("/color", Method.Post);
            addColorRequest.AddHeader("Authorization", $"Bearer {invalidToken}");
            addColorRequest.AddJsonBody(new
            {
                title = $"Color_{random.Next(999, 9999)}"
            });

            var addColorResponse = restClient.Execute(addColorRequest);

            //Assert that the request is false
            Assert.That(addColorResponse.IsSuccessful, Is.False);
            Assert.That(addColorResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
    }
}
