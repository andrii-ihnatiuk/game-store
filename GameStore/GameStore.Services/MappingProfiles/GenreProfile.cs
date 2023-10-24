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
        CreateMap<GenreCreateInnerDto, Genre>();
        CreateMap<GenreCreateDto, Genre>()
            .IncludeMembers(g => g.Genre)
            .ForMember(
                dest => dest.ParentGenreId,
                opts => opts.MapFrom(src => ConstructNullableGuidFromString(src.Genre.ParentGenreId)));

        CreateMap<Genre, GenreFullDto>();

        CreateMap<Genre, GenreBriefDto>();

        CreateMap<GenreUpdateInnerDto, Genre>();
        CreateMap<GenreUpdateDto, Genre>()
            .IncludeMembers(g => g.Genre)
            .ForMember(
                dest => dest.ParentGenreId,
                opts => opts.MapFrom(src => ConstructNullableGuidFromString(src.Genre.ParentGenreId)));
    }

    private static Guid? ConstructNullableGuidFromString(string? str)
    {
        return string.IsNullOrEmpty(str) ? null : new Guid(str);
    }
}