using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizationService.DTOs.LegalDocument;
using OrganizationService.Services.Interfaces;
using SharedLibrary.DTOs;

namespace OrganizationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LegalDocumentsController : ControllerBase
    {
        private readonly ILegalDocumentService _legalDocumentService;

        public LegalDocumentsController(ILegalDocumentService legalDocumentService)
        {
            _legalDocumentService = legalDocumentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLegalDocumentRequest request)
        {
            var result = await _legalDocumentService.CreateAsync(request);
            return Ok(ApiResponse<LegalDocumentResponse>.SuccessResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLegalDocumentRequest request)
        {
            var result = await _legalDocumentService.UpdateAsync(id, request);
            return Ok(ApiResponse<LegalDocumentResponse>.SuccessResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _legalDocumentService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _legalDocumentService.GetByIdAsync(id);
            return Ok(ApiResponse<LegalDocumentResponse>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] LegalDocumentFilterRequest filter)
        {
            var result = await _legalDocumentService.GetAllAsync(filter);
            return Ok(ApiResponse<PaginatedResponse<LegalDocumentResponse>>.SuccessResponse(result));
        }
    }
}