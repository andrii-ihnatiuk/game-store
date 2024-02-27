using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Services.MappingProfiles.ValueResolvers;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Extensions;

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
                opts => opts.MapFrom(src => src.Genre.ParentGenreId.ToNullableGuid()));

        CreateMap<Genre, GenreFullDto>();

        CreateMap<Genre, GenreBriefDto>();

        CreateMap<GenreUpdateInnerDto, Genre>();
        CreateMap<GenreUpdateDto, Genre>()
            .IncludeMembers(g => g.Genre)
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom<LocalizedValueResolver<string>, string>(src => src.Genre.Name))
            .ForMember(
                dest => dest.ParentGenreId,
                opts => opts.MapFrom(src => src.Genre.ParentGenreId.ToNullableGuid()));

        CreateMap<GenreTranslation, Genre>()
            .ForMember(dest => dest.Id, opts => opts.Ignore());

        CreateMap<GenreUpdateInnerDto, GenreTranslation>();
        CreateMap<GenreUpdateDto, GenreTranslation>()
            .IncludeMembers(src => src.Genre)
            .ForMember(dest => dest.Id, opts => opts.Ignore())
            .ForMember(dest => dest.CoreId, opts => opts.MapFrom(src => src.Genre.Id))
            .ForMember(dest => dest.LanguageCode, opts => opts.MapFrom(src => src.Culture));
    }
}