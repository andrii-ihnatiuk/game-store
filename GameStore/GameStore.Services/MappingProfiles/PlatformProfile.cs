using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<PlatformInnerDto, Platform>();
        CreateMap<PlatformCreateDto, Platform>()
            .IncludeMembers(p => p.Platform);

        CreateMap<Platform, PlatformFullDto>();

        CreateMap<Platform, PlatformBriefDto>();

        CreateMap<PlatformUpdateDto, Platform>();
    }
}