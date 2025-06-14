using AutoMapper;
using AuthService.DTOs.Company;
using AuthService.Models;

namespace OrganizationService.MappingProfiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<CreateCompanyRequest, Company>();
            CreateMap<UpdateCompanyRequest, Company>();

            CreateMap<Company, CompanyResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.ChangedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ChangedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
