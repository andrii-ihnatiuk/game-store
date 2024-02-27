using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Services.MappingProfiles.ValueResolvers;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<Publisher, PublisherBriefDto>();
        CreateMap<Publisher, PublisherFullDto>();

        CreateMap<PublisherCreateInnerDto, Publisher>();
        CreateMap<PublisherCreateDto, Publisher>()
            .IncludeMembers(p => p.Publisher);

        CreateMap<PublisherUpdateInnerDto, Publisher>();
        CreateMap<PublisherUpdateDto, Publisher>()
            .IncludeMembers(p => p.Publisher)
            .ForMember(
                dest => dest.CompanyName,
                opts => opts.MapFrom<LocalizedValueResolver<string>, string>(src => src.Publisher.CompanyName))
            .ForMember(
                dest => dest.Description,
                opts => opts.MapFrom<LocalizedValueResolver<string>, string>(src => src.Publisher.Description));

        CreateMap<PublisherTranslation, Publisher>()
            .ForMember(dest => dest.Id, opts => opts.Ignore());

        CreateMap<PublisherUpdateInnerDto, PublisherTranslation>();
        CreateMap<PublisherUpdateDto, PublisherTranslation>()
            .IncludeMembers(src => src.Publisher)
            .ForMember(dest => dest.Id, opts => opts.Ignore())
            .ForMember(dest => dest.CoreId, opts => opts.MapFrom(src => src.Publisher.Id))
            .ForMember(dest => dest.LanguageCode, opts => opts.MapFrom(src => src.Culture));
    }
}