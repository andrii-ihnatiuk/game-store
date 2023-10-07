using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.MappingProfiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreCreateDto, Genre>();

        CreateMap<Genre, GenreViewFullDto>()
            .ForMember(
                dest => dest.GenreId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<Genre, GenreViewBriefDto>()
            .ForMember(
                dest => dest.GenreId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<GenreUpdateDto, Genre>()
            .ForMember(
                dest => dest.Id,
                opts => opts.MapFrom(src => src.GenreId));
    }
}