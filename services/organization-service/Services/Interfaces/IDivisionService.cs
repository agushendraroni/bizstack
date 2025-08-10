using OrganizationService.DTOs.Division;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface IDivisionService
    {
        Task<DivisionResponse> CreateAsync(CreateDivisionRequest request);
        Task<DivisionResponse> UpdateAsync(Guid id, UpdateDivisionRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<DivisionResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<DivisionResponse>> GetAllAsync(DivisionFilterRequest filter);
    }
}