using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Models;

namespace GameStore.Shared.Interfaces.Services;

public interface IGameService : IResolvableByEntityStorage
{
    Task<GameFullDto> GetGameByAliasAsync(string alias);

    Task<GameFullDto> GetGameByIdAsync(string id);

    Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias);

    Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias);

    Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias);

    Task<EntityFilteringResult<GameFullDto>> GetAllGamesAsync(GamesFilter filter);

    Task<Tuple<byte[], string>> DownloadAsync(string gameAlias);
}