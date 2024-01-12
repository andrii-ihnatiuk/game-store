using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities.Identity;
using GameStore.Shared.DTOs.Role;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<ApplicationRole, RoleBriefDto>();

        CreateMap<RoleUpdateInnerDto, ApplicationRole>();
        CreateMap<RoleUpdateDto, ApplicationRole>()
            .IncludeMembers(src => src.Role);
    }
}