using AuthService.DTOs.User;
using SharedLibrary.DTOs;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var result = await _userService.CreateAsync(request);
            return Ok(ApiResponse<UserResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
        {
            var result = await _userService.UpdateAsync(id, request);
            return Ok(ApiResponse<UserResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(ApiResponse<UserResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterRequest filter)
        {
            var result = await _userService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<UserResponse>>.SuccessResponse(result));
        }
    }
}
