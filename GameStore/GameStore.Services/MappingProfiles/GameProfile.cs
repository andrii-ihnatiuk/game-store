using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Game;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameInnerDto, Game>();
        CreateMap<GameCreateDto, Game>()
            .IncludeMembers(src => src.Game)
            .ForMember(
                dest => dest.Publisher,
                opts => opts.Ignore())
            .ForMember(
                dest => dest.Alias,
                opts => opts.MapFrom(src => ConstructAlias(src)))
            .ForMember(
                dest => dest.Discontinued,
                opts => opts.MapFrom(src => src.Game.Discontinued != 0))
            .ForMember(
                dest => dest.GameGenres,
                opts => opts.MapFrom(src => ConstructGameGenresFromIds(src.Genres)))
            .ForMember(
                dest => dest.GamePlatforms,
                opts => opts.MapFrom(src => ConstructGamePlatformsFromIds(src.Platforms)))
            .ForMember(
                dest => dest.PublisherId,
                opts => opts.MapFrom(src => ConstructNullableGuidFromString(src.Publisher)));

        CreateMap<GameUpdateDto, Game>()
            .ForMember(
                dest => dest.Alias,
                opts => opts.MapFrom(src => src.Key))
            .ForMember(
                dest => dest.GameGenres,
                opts => opts.MapFrom(src => ConstructGameGenresFromIds(src.Genres)))
            .ForMember(
                dest => dest.GamePlatforms,
                opts => opts.MapFrom(src => ConstructGamePlatformsFromIds(src.Platforms)));

        CreateMap<Game, GameBriefDto>()
            .ForMember(
                dest => dest.Key,
                opts => opts.MapFrom(src => src.Alias));

        CreateMap<Game, GameFullDto>()
            .ForMember(
                dest => dest.Key,
                opts => opts.MapFrom(src => src.Alias));

        CreateMap<IEnumerable<Game>, GamesWithCountDto>()
            .ForMember(
                dest => dest.Games,
                opts => opts.MapFrom(src => src))
            .ForMember(
                dest => dest.Count,
                opts => opts.MapFrom(src => src.Count()));
    }

    private static string ConstructAlias(GameCreateDto dto)
    {
        return string.IsNullOrEmpty(dto.Game.Key)
            ? string.Join('-', dto.Game.Name.Split(' ')).ToLower()
            : dto.Game.Key;
    }

    private static IList<GameGenre> ConstructGameGenresFromIds(IEnumerable<Guid>? genres)
    {
        return genres is null
            ? new List<GameGenre>()
            : genres.Select(id => new GameGenre { GenreId = id }).ToList();
    }

    private static IList<GamePlatform> ConstructGamePlatformsFromIds(IEnumerable<Guid>? platforms)
    {
        return platforms is null
            ? new List<GamePlatform>()
            : platforms.Select(id => new GamePlatform { PlatformId = id }).ToList();
    }

    private static Guid? ConstructNullableGuidFromString(string? str)
    {
        return string.IsNullOrEmpty(str) ? null : new Guid(str);
    }
}