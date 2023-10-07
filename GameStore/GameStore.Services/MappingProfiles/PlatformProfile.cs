using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Services.MappingProfiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<PlatformCreateDto, Platform>();

        CreateMap<Platform, PlatformViewFullDto>()
            .ForMember(
                dest => dest.PlatformId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<Platform, PlatformViewBriefDto>()
            .ForMember(
                dest => dest.PlatformId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<PlatformUpdateDto, Platform>()
            .ForMember(
                dest => dest.Id,
                opts => opts.MapFrom(src => src.PlatformId));
    }
}