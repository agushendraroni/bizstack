using OrganizationService.DTOs.LegalDocument;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface ILegalDocumentService
    {
        Task<LegalDocumentResponse> CreateAsync(CreateLegalDocumentRequest request, int? tenantId = null, Guid? userId = null);
        Task<LegalDocumentResponse> UpdateAsync(Guid id, UpdateLegalDocumentRequest request);
        Task<bool> DeleteAsync(Guid id, int? tenantId = null, Guid? userId = null);
        Task<LegalDocumentResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<LegalDocumentResponse>> GetAllAsync(LegalDocumentFilterRequest filter);
    }
}