using GameStore.Shared.DTOs.Game;

namespace GameStore.Services.Services;

public interface IGameService
{
    Task<GameViewFullDto> GetGameByAliasAsync(string alias);

    Task<IList<GameViewBriefDto>> GetAllGamesAsync();

    Task<GameViewFullDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);

    Task DeleteGameAsync(long gameId);

    Task<Tuple<byte[], string>> DownloadAsync(string gameAlias);
}