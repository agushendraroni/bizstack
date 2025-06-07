using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly string _jwtSecret = "your_super_secret_key_123!"; // Simpan di config/env
        private readonly string _issuer = "auth-service";

        [HttpPost]
        public IActionResult GenerateToken([FromBody] TokenRequest request)
        {
            if(request.ServiceName != "user-service")
            {
                return Unauthorized("Service not allowed");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("sub", "auth-service"),
                    new Claim("aud", request.ServiceName),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { token = jwt });
        }
    }

    public class TokenRequest
    {
        public string? ServiceName { get; set; }
    }
}
