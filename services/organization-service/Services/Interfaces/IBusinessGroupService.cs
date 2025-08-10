using OrganizationService.DTOs.BusinessGroup;
using SharedLibrary.DTOs;

namespace OrganizationService.Services.Interfaces
{
    public interface IBusinessGroupService
    {
        Task<BusinessGroupResponse> CreateAsync(CreateBusinessGroupRequest request);
        Task<BusinessGroupResponse> UpdateAsync(Guid id, UpdateBusinessGroupRequest request);
        Task<bool> DeleteAsync(Guid id);
        Task<BusinessGroupResponse> GetByIdAsync(Guid id);
        Task<PaginatedResponse<BusinessGroupResponse>> GetAllAsync(BusinessGroupFilterRequest filter);
    }
}