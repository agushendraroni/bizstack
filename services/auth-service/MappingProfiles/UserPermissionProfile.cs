using AutoMapper;
using AuthService.DTOs;
using AuthService.Models;
using AuthService.DTOs.UserPermission;

namespace AuthService.Profiles
{
    public class UserPermissionProfile : Profile
    {
        public UserPermissionProfile()
        {
            CreateMap<CreateUserPermissionRequest, UserPermission>();
            CreateMap<UpdateUserPermissionRequest, UserPermission>();

            CreateMap<UserPermission, UserPermissionResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.PermissionId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ChangedAt, opt => opt.MapFrom(src => src.ChangedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.ChangedBy, opt => opt.MapFrom(src => src.ChangedBy))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));
        }
    }
}