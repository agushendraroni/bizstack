using OrganizationService.DTOs.CostCenter;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface ICostCenterService
    {
        Task<CostCenterResponse> CreateAsync(CreateCostCenterRequest request, int? tenantId = null, Guid? userId = null);
        Task<CostCenterResponse> UpdateAsync(Guid id, UpdateCostCenterRequest request);
        Task<bool> DeleteAsync(Guid id, int? tenantId = null, Guid? userId = null);
        Task<CostCenterResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<CostCenterResponse>> GetAllAsync(CostCenterFilterRequest filter);
    }
}