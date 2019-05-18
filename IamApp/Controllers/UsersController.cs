using IamApp.Domain;
using IamApp.Dtos;
using IamApp.Extensions;
using IamApp.Repositories;
using IamApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace IamApp.Controllers
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

        [AllowAnonymous] //TEMP
        [HttpGet]
        public ActionResult GetAll()
        {
            var users = _userRepository.GetAll();
            //var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(users.Select(user => new {
                user.Id,
                user.Username,
                user.Email,
            }));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login(LoginUserDto userDto)
        {
            if (userDto.Username.IsNullOrWhiteSpace()) return BadRequest(new { message = "Username is required." });
            if (userDto.Password.IsNullOrWhiteSpace()) return BadRequest(new { message = "Password is required." });

            var user = _userRepository.Authenticate(
                userDto.Username,
                userDto.Password
            );

            if (user == null) return BadRequest(new { message = "Username or password is incorrect." });

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                Token = new TokenGenerator().GenerateToken(_appSettings.Secret, user.Id)
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody]RegistrationUserDto userDto)
        {
            if (userDto.Username.IsNullOrWhiteSpace()) return BadRequest(new { message = "Username is required." });
            if (userDto.Password.IsNullOrWhiteSpace()) return BadRequest(new { message = "Password is required." });
            if (userDto.Email.IsNullOrWhiteSpace()) return BadRequest(new { message = "Email is required." });

            try
            {
                var user = new User(
                    userDto.Username,
                    userDto.Password,
                    userDto.Email
                );

                _userRepository.Save(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            };
        }
    }
}