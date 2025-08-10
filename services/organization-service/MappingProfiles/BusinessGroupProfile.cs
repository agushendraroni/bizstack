using AutoMapper;
using OrganizationService.DTOs.BusinessGroup;
using OrganizationService.Models;

namespace OrganizationService.MappingProfiles
{
    public class BusinessGroupProfile : Profile
    {
        public BusinessGroupProfile()
        {
            CreateMap<CreateBusinessGroupRequest, BusinessGroup>();
            CreateMap<UpdateBusinessGroupRequest, BusinessGroup>();
            CreateMap<BusinessGroup, BusinessGroupResponse>();
        }
    }
}