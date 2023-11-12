using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Models;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Game;

namespace GameStore.Application.MappingProfiles;

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
                opts => opts.MapFrom(src => src.Page ?? 1));
    }

    private static int GetNumericPageCount(string? pageCount)
    {
        string option = pageCount ?? PaginationOption.Ten;
        return PaginationOption.ToNumber(option);
    }
}