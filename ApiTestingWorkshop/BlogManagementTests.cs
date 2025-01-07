using Newtonsoft.Json.Linq;
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
    public class BlogManagementTests
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
           ,  "admin@gmail.com");
            random = new Random();  

        }
        [Test]
        public void BlogPostLifecycleTest()
        {
            //create blog post 
            var creatBlogPostRequest = new RestRequest("/blog", Method.Post);
            creatBlogPostRequest.AddHeader("Authorization", $"Bearer {token}");
            creatBlogPostRequest.AddJsonBody(new {
            Title = $"BlogTitle_{random.Next(999, 9999).ToString()}",
            Description = "Description",
            Category = "Some Category"
            
            });

            //Start request
            var createBlogResposne = restClient.Execute(creatBlogPostRequest);

            //Extract ID form response
            string blogID = JObject.Parse(createBlogResposne.Content)["id"]?.ToString();

            //Assert That everiting  is StatusCode OK or not  null
            Assert.That(createBlogResposne.IsSuccessful, Is.True, "createBlogResposne failed");
            Assert.That(blogID, Is.Not.Empty.Or.Null, "blogID is Empty");

            //Update Blog Post   
            var updateRequest = new RestRequest($"/blog/{blogID}", Method.Put);
            updateRequest.AddHeader("Authorization", $"Bearer{token}" );
            updateRequest.AddJsonBody(new
            {
                Title = $"BlogTitle_{random.Next(999, 9999)}",
                Description = "UpdatedDescription",
                Category = "Some CategoryOne"

            });
            var updateResposne = restClient.Execute(updateRequest);

            //Assert that request is status code OK
            Assert.That(updateResposne.IsSuccessful, Is.True, "updateResposne failed");  

            //Detele Blog Post
            var deleteRequest = new RestRequest($"/blog/{blogID}" , Method.Delete);
            deleteRequest.AddHeader("Authorization", $"Bearer{token}");

            //Send the request
            var deleteResponse = restClient.Execute(deleteRequest);

            //Assert that
            Assert.That(deleteResponse.IsSuccessful, Is.True, "deleteResponse is not deleted succsessfuly");


            //Make get request to check for the deleted blog
            var verifyRequest = new RestRequest($"/blog/{blogID}", Method.Get);
            var verifyResponse = restClient.Execute(verifyRequest);

            //Assetr to chek for Null
            Assert.That(verifyResponse.Content, Is.Null.Or.EqualTo("null "), "Verify response faeld");
        }
    }
}
