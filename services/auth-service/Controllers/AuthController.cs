using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using AuthService.DTOs;
using AuthService.Services;
using SharedLibrary.DTOs;

namespace AuthService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
                return Unauthorized(new ApiResponse<LoginResponse> { Data = null, IsSuccess = false, Message = "Invalid username or password" });

            return Ok(new ApiResponse<LoginResponse> { Data = result, IsSuccess = true, Message = "Login successful" });
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            
            if (result == null)
                return Unauthorized(new ApiResponse<LoginResponse> { Data = null, IsSuccess = false, Message = "Invalid refresh token" });

            return Ok(new ApiResponse<LoginResponse> { Data = result, IsSuccess = true, Message = "Token refreshed" });
        }

        [HttpPost("logout")]
        public async Task<ActionResult<ApiResponse<string>>> Logout([FromBody] string refreshToken)
        {
            var success = await _authService.LogoutAsync(refreshToken);
            
            if (!success)
                return BadRequest(new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Invalid refresh token" });

            return Ok(new ApiResponse<string> { Data = "Logged out successfully", IsSuccess = true });
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterRequest request)
        {
            var success = await _authService.RegisterAsync(request.Username, request.Password, request.CompanyId);
            
            if (!success)
                return BadRequest(new ApiResponse<string> { Data = null, IsSuccess = false, Message = "Username already exists" });

            return Ok(new ApiResponse<string> { Data = "User registered successfully", IsSuccess = true });
        }

        private Guid? GetUserId()
        {
            if (Request.Headers.TryGetValue("X-User-Id", out var userIdHeader) && 
                Guid.TryParse(userIdHeader.FirstOrDefault(), out var userId))
            {
                return userId;
            }
            return null;
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Guid? CompanyId { get; set; }
    }
}