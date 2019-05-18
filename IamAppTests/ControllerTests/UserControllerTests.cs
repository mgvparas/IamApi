using IamApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace IamAppTests
{
    public class UserControllerTests
    {
        [Fact]
        public void IsSuccessful_Register()
        {
            var api = new UserController();
            var postResponse = (ObjectResult)api.Register();

            Assert.Equal((int)HttpStatusCode.Created, postResponse.StatusCode);
        }
    }
}
