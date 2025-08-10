using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationService.DTOs.JobTitle;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class JobTitlesController : ControllerBase
    {
        private readonly IJobTitleService _jobTitleService;

        public JobTitlesController(IJobTitleService jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobTitleRequest request)
        {
            var result = await _jobTitleService.CreateAsync(request);
            return Ok(ApiResponse<JobTitleResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJobTitleRequest request)
        {
            var result = await _jobTitleService.UpdateAsync(id, request);
            return Ok(ApiResponse<JobTitleResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _jobTitleService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _jobTitleService.GetByIdAsync(id);
            return Ok(ApiResponse<JobTitleResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] JobTitleFilterRequest filter)
        {
            var result = await _jobTitleService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<JobTitleResponse>>.SuccessResponse(result));
        }
    }
}