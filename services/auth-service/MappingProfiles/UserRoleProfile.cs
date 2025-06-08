using AutoMapper;
using AuthService.DTOs.UserRole;
using AuthService.Models;

namespace AuthService.Mappings
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<CreateUserRoleRequest, UserRole>();
            CreateMap<UpdateUserRoleRequest, UserRole>();

            CreateMap<UserRole, UserRoleResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.ChangedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ChangedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}