using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Services.MappingProfiles.ValueResolvers;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Services.MappingProfiles;

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
            .ForMember(dest => dest.Type, opts => opts.MapFrom<LocalizedValueResolver<string>, string>(src => src.Platform.Type))
            .IncludeMembers(p => p.Platform);

        CreateMap<PlatformTranslation, Platform>()
            .ForMember(dest => dest.Id, opts => opts.Ignore());

        CreateMap<PlatformUpdateInnerDto, PlatformTranslation>();
        CreateMap<PlatformUpdateDto, PlatformTranslation>()
            .IncludeMembers(src => src.Platform)
            .ForMember(dest => dest.Id, opts => opts.Ignore())
            .ForMember(dest => dest.CoreId, opts => opts.MapFrom(src => src.Platform.Id))
            .ForMember(dest => dest.LanguageCode, opts => opts.MapFrom(src => src.Culture));
    }
}