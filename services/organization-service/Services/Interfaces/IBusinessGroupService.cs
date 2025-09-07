using OrganizationService.DTOs.BusinessGroup;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface IBusinessGroupService
    {
        Task<BusinessGroupResponse> CreateAsync(CreateBusinessGroupRequest request, int? tenantId = null, Guid? userId = null);
        Task<BusinessGroupResponse> UpdateAsync(Guid id, UpdateBusinessGroupRequest request);
        Task<bool> DeleteAsync(Guid id, int? tenantId = null, Guid? userId = null);
        Task<BusinessGroupResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<BusinessGroupResponse>> GetAllAsync(BusinessGroupFilterRequest filter);
    }
}