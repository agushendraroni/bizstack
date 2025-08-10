using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationService.DTOs.Branch;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchesController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchRequest request)
        {
            var result = await _branchService.CreateAsync(request);
            return Ok(ApiResponse<BranchResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBranchRequest request)
        {
            var result = await _branchService.UpdateAsync(id, request);
            return Ok(ApiResponse<BranchResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _branchService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _branchService.GetByIdAsync(id);
            return Ok(ApiResponse<BranchResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BranchFilterRequest filter)
        {
            var result = await _branchService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<BranchResponse>>.SuccessResponse(result));
        }
    }
}