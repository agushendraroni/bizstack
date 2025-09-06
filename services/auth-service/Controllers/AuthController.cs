using Microsoft.AspNetCore.Mvc;
using AuthService.DTOs;
using AuthService.Services;
using SharedLibrary.DTOs;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            
            if (result == null)
                return Unauthorized(ApiResponse<LoginResponse>.Error("Invalid username or password"));

            return Ok(ApiResponse<LoginResponse>.Success(result, "Login successful"));
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            
            if (result == null)
                return Unauthorized(ApiResponse<LoginResponse>.Error("Invalid refresh token"));

            return Ok(ApiResponse<LoginResponse>.Success(result, "Token refreshed"));
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<string>>> Logout([FromBody] string refreshToken)
        {
            var success = await _authService.LogoutAsync(refreshToken);
            
            if (!success)
                return BadRequest(ApiResponse<string>.Error("Invalid refresh token"));

            return Ok(ApiResponse<string>.Success("Logged out successfully"));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterRequest request)
        {
            var success = await _authService.RegisterAsync(request.Username, request.Password, request.CompanyId);
            
            if (!success)
                return BadRequest(ApiResponse<string>.Error("Username already exists"));

            return Ok(ApiResponse<string>.Success("User registered successfully"));
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Guid? CompanyId { get; set; }
    }
}
