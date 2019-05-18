using IamApp.Domain;
using IamApp.Dtos;
using IamApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IamApp.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(UserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        protected UserRepository UserRepository { get; private set; }

        [HttpPost("register")]
        public ActionResult Register([FromBody]RegisterDto registerDto)
        {
            try
            {
                var user = new User(
                    registerDto.Username,
                    registerDto.Password
                );

                UserRepository.Save(user);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            };
        }
    }
}