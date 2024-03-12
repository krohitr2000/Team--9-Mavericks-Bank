//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using MavericksBank.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//namespace MavericksBank.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly IConfiguration _config;

//        public AuthController(IConfiguration config)
//        {
//            _config = config;
//        }

//        [AllowAnonymous]
//        [HttpPost]
//        public IActionResult Auth([FromBody] Customer customer)
//        {
//            var issuer = _config["Jwt:Issuer"];
//            var audience = _config["Jwt:Audience"];
//            Console.WriteLine(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
//            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
//            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
//            var subject = new ClaimsIdentity(
//                new[]
//                {
//                            new Claim(JwtRegisteredClaimNames.Sub, customer.Username),
//                            new Claim(JwtRegisteredClaimNames.Email, customer.Username),
//                });
//            var expires = DateTime.UtcNow.AddMinutes(10);
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = subject,
//                SigningCredentials = signingCredentials,
//                Expires = expires,
//                Issuer = issuer,
//                Audience = audience
//            };
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var jwtTokens = tokenHandler.WriteToken(token);
//            return Ok(jwtTokens);
//        }
//    }
//    }

using MavericksBank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MavericksBankDb2Context _context;

        public AuthController(MavericksBankDb2Context context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }

            var customer = _context.Customers.FirstOrDefault(c => c.Username == user.UserName);

            if (customer != null && VerifyPassword(customer, user.Password))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Customer")
                };

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:5126/",
                    audience: "http://localhost:5126/",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new Customer { Token = tokenString });
            }

            return Unauthorized();
        }
        private bool VerifyPassword(Customer customer, string password)
        {
            return customer.Password == password;
        }
    }
}
