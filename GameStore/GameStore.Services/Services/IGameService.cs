using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Services.Services;

public interface IGameService
{
    Task<GameFullDto> GetGameByAliasAsync(string alias);

    Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias);

    Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias);

    Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias);

    Task<IList<GameBriefDto>> GetAllGamesAsync();

    Task<GameFullDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);

    Task DeleteGameAsync(Guid gameId);

    Task<Tuple<byte[], string>> DownloadAsync(string gameAlias);
}