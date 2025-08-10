using OrganizationService.DTOs.LegalDocument;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface ILegalDocumentService
    {
        Task<LegalDocumentResponse> CreateAsync(CreateLegalDocumentRequest request);
        Task<LegalDocumentResponse> UpdateAsync(Guid id, UpdateLegalDocumentRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<LegalDocumentResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<LegalDocumentResponse>> GetAllAsync(LegalDocumentFilterRequest filter);
    }
}