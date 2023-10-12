using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreCreateDto, Genre>();

        CreateMap<Genre, GenreFullDto>()
            .ForMember(
                dest => dest.GenreId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<Genre, GenreBriefDto>()
            .ForMember(
                dest => dest.GenreId,
                opts => opts.MapFrom(src => src.Id));

        CreateMap<GenreUpdateDto, Genre>()
            .ForMember(
                dest => dest.Id,
                opts => opts.MapFrom(src => src.GenreId));
    }
}