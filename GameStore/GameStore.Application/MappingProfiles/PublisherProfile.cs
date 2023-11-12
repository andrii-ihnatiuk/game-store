using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Application.MappingProfiles;

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
            .IncludeMembers(p => p.Publisher);
    }
}