using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;
using UserService.DTOs.UserActivityLog;
using UserService.Services.Interfaces;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserActivityLogsController : ControllerBase
{
    private readonly IUserActivityLogService _service;

    public UserActivityLogsController(IUserActivityLogService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserActivityLogRequest request)
    {
        var result = await _service.CreateAsync(request, "system");
        return Ok(ApiResponse<UserActivityLogResponse>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound(SharedLibrary.DTOs.ApiResponse<string>.Fail("Not found"));
        return Ok(ApiResponse<UserActivityLogResponse>.SuccessResponse(result));
    }


    [HttpGet]
    public async Task<IActionResult> GetFiltered([FromQuery] UserActivityLogFilterRequest filter)
    {
        var result = await _service.GetFilteredAsync(filter);
        return Ok(ApiResponse<PaginatedResponse<UserActivityLogResponse>>.SuccessResponse(result));
    }
}