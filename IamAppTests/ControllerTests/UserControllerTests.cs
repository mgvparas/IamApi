using IamApp.Controllers;
using IamApp.Dtos;
using IamApp.Repositories;
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
            var repository = new UserRepository();
            var api = new UserController(repository);
            var postResponse = (StatusCodeResult)api.Register(
                new RegisterDto
                {
                    Username = "johndoe",
                    Password = "Asdf1234",
                }
            );

            Assert.Equal((int)HttpStatusCode.OK, postResponse.StatusCode);
        }
    }
}
