
using AuthService.DTOs.Common;
using AuthService.DTOs.UserPasswordHistory;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserPasswordHistoriesController : ControllerBase
    {
        private readonly IUserPasswordHistoryService _service;

        public UserPasswordHistoriesController(IUserPasswordHistoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserPasswordHistoryRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(ApiResponse<UserPasswordHistoryResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserPasswordHistoryRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<UserPasswordHistoryResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(result ? "Deleted" : "Not Found"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(ApiResponse<UserPasswordHistoryResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<UserPasswordHistoryResponse>>.SuccessResponse(result));
        }
    }
}