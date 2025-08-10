using OrganizationService.DTOs.Branch;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface IBranchService
    {
        Task<BranchResponse> CreateAsync(CreateBranchRequest request);
        Task<BranchResponse> UpdateAsync(Guid id, UpdateBranchRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<BranchResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<BranchResponse>> GetAllAsync(BranchFilterRequest filter);
    }
}