using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Services.MappingProfiles.ValueResolvers;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Extensions;

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
                dest => dest.GameGenres,
                opts => opts.MapFrom(src => ConstructGameGenresFromIds(src.Genres, Guid.Empty.ToString())))
            .ForMember(
                dest => dest.GamePlatforms,
                opts => opts.MapFrom(src => ConstructGamePlatformsFromIds(src.Platforms, Guid.Empty.ToString())))
            .ForMember(
                dest => dest.PublisherId,
                opts => opts.MapFrom(src => src.Publisher.ToNullableGuid()));

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
                dest => dest.GameGenres,
                opts => opts.MapFrom(src => ConstructGameGenresFromIds(src.Genres, src.Game.Id)))
            .ForMember(
                dest => dest.GamePlatforms,
                opts => opts.MapFrom(src => ConstructGamePlatformsFromIds(src.Platforms, src.Game.Id)))
            .ForMember(
                dest => dest.PublisherId,
                opts => opts.MapFrom(src => src.Publisher.ToNullableGuid()))
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom<LocalizedValueResolver<string>, string>(src => src.Game.Name))
            .ForMember(
                dest => dest.Type,
                opts => opts.MapFrom<LocalizedValueResolver<string?>, string>(src => src.Game.Type))
            .ForMember(
                dest => dest.Description,
                opts => opts.MapFrom<LocalizedValueResolver<string?>, string?>(src => src.Game.Description));

        CreateMap<Game, GameBriefDto>()
            .ForMember(
                dest => dest.Key,
                opts => opts.MapFrom(src => src.Alias));

        CreateMap<Game, GameFullDto>()
            .ForMember(
                dest => dest.Key,
                opts => opts.MapFrom(src => src.Alias));

        CreateMap<GameTranslation, Game>()
            .ForMember(dest => dest.Id, opts => opts.Ignore());

        CreateMap<GameUpdateInnerDto, GameTranslation>();
        CreateMap<GameUpdateDto, GameTranslation>()
            .IncludeMembers(src => src.Game)
            .ForMember(dest => dest.Id, opts => opts.Ignore())
            .ForMember(dest => dest.CoreId, opts => opts.MapFrom(src => src.Game.Id))
            .ForMember(dest => dest.LanguageCode, opts => opts.MapFrom(src => src.Culture));
    }

    private static string ConstructAliasIfEmpty(string? alias, string source)
    {
        return string.IsNullOrEmpty(alias)
            ? string.Join('-', source.Split(' ')).ToLower()
            : alias;
    }

    private static IList<GameGenre> ConstructGameGenresFromIds(IEnumerable<string>? genres, string gameId)
    {
        return genres is null
            ? new List<GameGenre>()
            : genres.Select(id => new GameGenre { GenreId = Guid.Parse(id), GameId = Guid.Parse(gameId) }).ToList();
    }

    private static IList<GamePlatform> ConstructGamePlatformsFromIds(IEnumerable<Guid>? platforms, string gameId)
    {
        return platforms is null
            ? new List<GamePlatform>()
            : platforms.Select(id => new GamePlatform { PlatformId = id, GameId = Guid.Parse(gameId) }).ToList();
    }
}