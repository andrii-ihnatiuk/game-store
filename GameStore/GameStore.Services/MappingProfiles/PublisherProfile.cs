using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Services.MappingProfiles;

public class PublisherProfile : Profile
{
    public PublisherProfile()
    {
        CreateMap<Publisher, PublisherBriefDto>();
    }
}