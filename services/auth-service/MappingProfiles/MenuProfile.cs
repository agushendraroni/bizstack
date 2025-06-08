// ==========================
// Mappings/MenuProfile.cs
// ==========================
using AutoMapper;
using AuthService.DTOs.Menu;
using AuthService.Models;

namespace AuthService.Mappings
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<CreateMenuRequest, Menu>();
            CreateMap<UpdateMenuRequest, Menu>();

            CreateMap<Menu, MenuResponse>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon))
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.OrderIndex))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.ChangedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ChangedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
        }
    }
}
