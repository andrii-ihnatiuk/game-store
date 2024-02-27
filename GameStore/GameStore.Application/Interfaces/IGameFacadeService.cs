using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Application.Interfaces;

public interface IGameFacadeService
{
    Task<FilteredGamesDto> GetFilteredGamesAsync(GamesFilterDto filterDto, string culture, bool showDeleted = false);

    Task<GameFullDto> GetGameByIdAsync(string id, string culture);

    Task<GameFullDto> GetGameByAliasAsync(string alias, string culture);

    Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias);

    Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias);

    Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias);

    Task<GameBriefDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);

    Task DeleteGameAsync(string alias);

    Task<Tuple<byte[], string>> DownloadAsync(string gameAlias);
}