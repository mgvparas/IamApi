using IamApi.Domain;
using IamApi.Dtos;
using IamApi.Repositories;
using IamApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace IamApi.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly UserRepository _userRepository;

        public UsersController(
            IUserRepository userRepository,
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _userRepository = (UserRepository)userRepository;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = _userRepository.GetAll();

            return Ok(users.Select(user => new {
                user.Id,
                user.Email,
            }));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login(LoginUserDto userDto)
        {
            try
            {
                var user = _userRepository.Authenticate(
                    userDto.Email,
                    userDto.Password
                );

                if (user == null) return BadRequest(new { message = "Email or password is incorrect." });

                return Ok(new
                {
                    message = "Login successful!",
                    user.Id,
                    user.Email,
                    Token = new TokenGenerator().GenerateToken(_appSettings.Secret, user.Id)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody]RegistrationUserDto userDto)
        {
            try
            {
                var user = new User(
                    userDto.Email,
                    userDto.Password
                );

                _userRepository.Save(user);

                return Ok(new { message = "Account successfully created!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }

        [HttpPost("updatePassword")]
        public ActionResult UpdatePassword([FromBody]RegistrationUserDto userDto)
        {
            try
            {
                var user = new User(
                    userDto.Email,
                    userDto.Password
                );

                _userRepository.Save(user);

                return Ok(new { message = "Account successfully created!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }
    }
}