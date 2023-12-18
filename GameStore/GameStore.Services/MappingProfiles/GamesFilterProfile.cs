using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Extensions;
using GameStore.Shared.Models;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class GamesFilterProfile : Profile
{
    public GamesFilterProfile()
    {
        CreateMap<GamesFilterDto, GamesFilter>()
            .ForMember(
                dest => dest.PageCount,
                opts => opts.MapFrom(src => GetNumericPageCount(src.PageCount)))
            .ForMember(
                dest => dest.Page,
                opts => opts.MapFrom(src => src.Page ?? 1))
            .ForMember(
                dest => dest.Genres,
                opts => opts.MapFrom(src => src.Genres.Where(i => i.IsGuidFormat())))
            .ForMember(
                dest => dest.Publishers,
                opts => opts.MapFrom(src => src.Publishers.Where(i => i.IsGuidFormat())))
            .ForMember(
                dest => dest.MongoCategories,
                opts => opts.MapFrom(src => src.Genres.Where(i => i.IsNotGuidFormat())))
            .ForMember(
                dest => dest.MongoSuppliers,
                opts => opts.MapFrom(src => src.Publishers.Where(i => i.IsNotGuidFormat())))
            .AfterMap((_, filter) => filter.Limit = filter.Page * filter.PageCount);
    }

    private static int GetNumericPageCount(string? pageCount)
    {
        string option = pageCount ?? PaginationOption.Ten;
        return PaginationOption.ToNumber(option);
    }
}