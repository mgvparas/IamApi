using IamApp.Controllers;
using IamApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace IamAppTests
{
    public class UserControllerTests
    {
        //[Fact]
        //public void Successful_Register()
        //{
        //    var repository = new UserTestRepository();
        //    var api = new UserController(repository, );
        //    var postResponse = (StatusCodeResult)api.Register(
        //        new RegistrationUserDto
        //        {
        //            Username = "johndoe",
        //            Password = "Asdf1234",
        //        }
        //    );

        //    Assert.Equal((int)HttpStatusCode.OK, postResponse.StatusCode);
        //}

        //[Fact]
        //public void Successful_Login()
        //{
        //    var repository = new UserTestRepository();
        //    var api = new UserController(repository);
        //    var registerResponse = (StatusCodeResult)api.Register(
        //        new RegistrationUserDto
        //        {
        //            Username = "johndoe",
        //            Password = "Asdf1234",
        //        }
        //    );

        //    Assert.Equal((int)HttpStatusCode.OK, registerResponse.StatusCode);

        //    var loginResponse = (StatusCodeResult)api.Login(
        //        new LoginUserDto
        //        {
        //            Username = "johndoe",
        //            Password = "Asdf1234"
        //        }
        //    );

        //    Assert.Equal((int)HttpStatusCode.OK, registerResponse.StatusCode);
        //}
    }
}
