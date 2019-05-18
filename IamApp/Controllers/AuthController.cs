using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IamApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public ActionResult GetToken()
        {
            var securityKey = "`1234567890-=~!@#$%^&*()_=qwertyuiop[]\\{}|asdfghjkl;:\"'zxcvbnm,./<>?";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim("CustomClaim", "CustomClaim"),
            };

            var token = new JwtSecurityToken(
                issuer: "test.issuer",
                audience: "readers",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials,
                claims: claims
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}