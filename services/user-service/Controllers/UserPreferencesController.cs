
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;
using UserService.DTOs.UserPreference;
using UserService.Services.Interfaces;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserPreferencesController : ControllerBase
{
    private readonly IUserPreferenceService _service;

    public UserPreferencesController(IUserPreferenceService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserPreferenceRequest request)
    {
        var result = await _service.CreateAsync(request, "system");
        return Ok(ApiResponse<UserPreferenceResponse>.SuccessResponse(result));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserPreferenceRequest request)
    {
        var result = await _service.UpdateAsync(id, request, "system");
        return Ok(ApiResponse<UserPreferenceResponse>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound(ApiResponse<string>.Fail("Not found"));
        return Ok(ApiResponse<UserPreferenceResponse>.SuccessResponse(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] UserPreferenceFilterRequest filter)
    {
        var result = await _service.GetFilteredAsync(filter);
        return Ok(ApiResponse<PaginatedResponse<UserPreferenceResponse>>.SuccessResponse(result));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound(ApiResponse<string>.Fail("Not found"));
        return Ok(ApiResponse<string>.SuccessResponse("Deleted"));
    }
}