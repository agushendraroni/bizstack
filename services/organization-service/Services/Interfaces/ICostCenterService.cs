using OrganizationService.DTOs.CostCenter;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface ICostCenterService
    {
        Task<CostCenterResponse> CreateAsync(CreateCostCenterRequest request);
        Task<CostCenterResponse> UpdateAsync(Guid id, UpdateCostCenterRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<CostCenterResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<CostCenterResponse>> GetAllAsync(CostCenterFilterRequest filter);
    }
}