using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
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
            token = GlobalConstants.AuthenticateUser("admin@gmail.com"
           , "admin@gmail.com");
            random = new Random();

        }
        [Test]
        public void ColorLifecycleTest()
        {
            //Make post request with a random title
            var addColorRequest = new RestRequest("/color", Method.Post);
            addColorRequest.AddHeader("Authorization", $"Bearer{token}");
            addColorRequest.AddJsonBody(new
            {
                title =$"Color_{random.Next(999, 9999 )}"
            });

            var addColorResponse = restClient.Execute(addColorRequest);

            //Assert that is ready
            Assert.That(addColorResponse.IsSuccessful, Is.True, "The request color failed");
        }

    }
}
