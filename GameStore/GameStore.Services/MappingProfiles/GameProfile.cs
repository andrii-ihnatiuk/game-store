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
        CreateMap<GameCreateInnerDto, Game>();
        CreateMap<GameCreateDto, Game>()
            .IncludeMembers(src => src.Game)
            .ForMember(
                dest => dest.Publisher,
                opts => opts.Ignore())
            .ForMember(
                dest => dest.Alias,
                opts => opts.MapFrom(src => ConstructAliasIfEmpty(src.Game.Key, src.Game.Name)))
            .ForMember(
                dest => dest.Discontinued,
                opts => opts.MapFrom(src => src.Game.Discontinued != 0))
            .ForMember(
                dest => dest.GameGenres,
                opts => opts.MapFrom(src => ConstructGameGenresFromIds(src.Genres, Guid.Empty)))
            .ForMember(
                dest => dest.GamePlatforms,
                opts => opts.MapFrom(src => ConstructGamePlatformsFromIds(src.Platforms, Guid.Empty)))
            .ForMember(
                dest => dest.PublisherId,
                opts => opts.MapFrom(src => ConstructNullableGuidFromString(src.Publisher)));

        CreateMap<GameUpdateInnerDto, Game>();
        CreateMap<GameUpdateDto, Game>()
            .IncludeMembers(src => src.Game)
            .ForMember(
                dest => dest.Publisher,
                opts => opts.Ignore())
            .ForMember(
                dest => dest.Alias,
                opts => opts.MapFrom(src => ConstructAliasIfEmpty(src.Game.Key, src.Game.Name)))
            .ForMember(
                dest => dest.Discontinued,
                opts => opts.MapFrom(src => src.Game.Discontinued != 0))
            .ForMember(
                dest => dest.GameGenres,
                opts => opts.MapFrom(src => ConstructGameGenresFromIds(src.Genres, src.Game.Id)))
            .ForMember(
                dest => dest.GamePlatforms,
                opts => opts.MapFrom(src => ConstructGamePlatformsFromIds(src.Platforms, src.Game.Id)))
            .ForMember(
                dest => dest.PublisherId,
                opts => opts.MapFrom(src => ConstructNullableGuidFromString(src.Publisher)));

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

    private static string ConstructAliasIfEmpty(string? alias, string source)
    {
        return string.IsNullOrEmpty(alias)
            ? string.Join('-', source.Split(' ')).ToLower()
            : alias;
    }

    private static IList<GameGenre> ConstructGameGenresFromIds(IEnumerable<Guid>? genres, Guid gameId)
    {
        return genres is null
            ? new List<GameGenre>()
            : genres.Select(id => new GameGenre { GenreId = id, GameId = gameId }).ToList();
    }

    private static IList<GamePlatform> ConstructGamePlatformsFromIds(IEnumerable<Guid>? platforms, Guid gameId)
    {
        return platforms is null
            ? new List<GamePlatform>()
            : platforms.Select(id => new GamePlatform { PlatformId = id, GameId = gameId }).ToList();
    }

    private static Guid? ConstructNullableGuidFromString(string? str)
    {
        return string.IsNullOrEmpty(str) ? null : new Guid(str);
    }
}