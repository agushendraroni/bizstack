using AutoMapper;
using OrganizationService.DTOs.CostCenter;
using OrganizationService.Models;

namespace OrganizationService.MappingProfiles
{
    public class CostCenterProfile : Profile
    {
        public CostCenterProfile()
        {
            CreateMap<CreateCostCenterRequest, CostCenter>();
            CreateMap<UpdateCostCenterRequest, CostCenter>();
            CreateMap<CostCenter, CostCenterResponse>();
        }
    }
}