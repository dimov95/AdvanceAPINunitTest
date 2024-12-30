using NUnit.Framework;
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
        restClient.Dispose();
        }
        [SetUp]
        public void Setup() 
        {
        restClient = new RestSharp.RestClient(GlobalConstants.BaseUrl);
            token = GlobalConstants.AuthenticateUser("admin@gmail.com"
           ,  "admin@gmail.com");
            random = new Random();  

        }
    }
}
