﻿using IamApp.Domain;
using IamApp.Dtos;
using IamApp.Extensions;
using IamApp.Repositories;
using IamApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.Contracts;

namespace IamApp.Controllers
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserRepository _userRepository;

        public UserController(
            IUserRepository userRepository,
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login(LoginUserDto loginUserDto)
        {
            var user = _userRepository.Authenticate(
                loginUserDto.Username,
                loginUserDto.Password
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
            Contract.Requires(!userDto.Username.IsNullOrWhiteSpace(), "Username is required.");
            Contract.Requires(!userDto.Password.IsNullOrWhiteSpace(), "Password is required.");
            Contract.Requires(!userDto.Email.IsNullOrWhiteSpace(), "Email is required.");

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