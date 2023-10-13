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

        CreateMap<Platform, PlatformFullDto>()
            .ForMember(
                dest => dest.PlatformId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<Platform, PlatformBriefDto>()
            .ForMember(
                dest => dest.PlatformId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<PlatformUpdateDto, Platform>()
            .ForMember(
                dest => dest.Id,
                opts => opts.MapFrom(src => src.PlatformId));
    }
}