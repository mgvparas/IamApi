using IamApp.Domain;
using IamApp.Dtos;
using IamApp.Repositories;
using IamApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
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
            try
            {
                var user = _userRepository.Authenticate(
                    userDto.Username,
                    userDto.Password
                );

                if (user == null) return BadRequest(new { message = "Username or password is incorrect." });

                return Ok(new
                {
                    message = "Login successful!",
                    user.Id,
                    user.Username,
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
                    userDto.Username,
                    userDto.Password,
                    userDto.Email
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