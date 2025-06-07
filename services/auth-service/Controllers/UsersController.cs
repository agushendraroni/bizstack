using AuthService.DTOs.Common;
using AuthService.DTOs.User;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterRequest filter)
        {
            var users = await _service.GetAllAsync(filter);
            return Ok(ApiResponse<IEnumerable<UserResponse>>.SuccessResponse(users));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<object>.Fail("User not found"));

            return Ok(ApiResponse<UserResponse>.SuccessResponse(user));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.Fail("Validation failed", errors));
            }

            try
            {
                var createdUser = await _service.CreateAsync(request, GetUserId());
                if (createdUser == null)
                    return BadRequest(ApiResponse<object>.Fail("User creation failed"));

                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id },
                    ApiResponse<UserResponse>.SuccessResponse(createdUser, "User created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error", new[] { ex.Message }));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return BadRequest(ApiResponse<object>.Fail("Validation failed", errors));
            }

            try
            {
                var updatedUser = await _service.UpdateAsync(id, request, GetUserId());
                if (updatedUser == null)
                    return NotFound(ApiResponse<object>.Fail("User not found"));

                return Ok(ApiResponse<UserResponse>.SuccessResponse(updatedUser, "User updated successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail("Internal server error", new[] { ex.Message }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id, GetUserId());
            if (!deleted)
                return NotFound(ApiResponse<object>.Fail("User not found"));

            return NoContent();
        }
    }
}