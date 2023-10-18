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
        CreateMap<PlatformCreateDto, Platform>();

        CreateMap<Platform, PlatformFullDto>();

        CreateMap<Platform, PlatformBriefDto>();

        CreateMap<PlatformUpdateDto, Platform>();
    }
}