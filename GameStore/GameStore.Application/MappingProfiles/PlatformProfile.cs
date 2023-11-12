using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Application.MappingProfiles;

[ExcludeFromCodeCoverage]
public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<PlatformCreateInnerDto, Platform>();
        CreateMap<PlatformCreateDto, Platform>()
            .IncludeMembers(p => p.Platform);

        CreateMap<Platform, PlatformFullDto>();

        CreateMap<Platform, PlatformBriefDto>();

        CreateMap<PlatformUpdateInnerDto, Platform>();
        CreateMap<PlatformUpdateDto, Platform>()
            .IncludeMembers(p => p.Platform);
    }
}