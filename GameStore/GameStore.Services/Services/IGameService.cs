using GameStore.Shared.DTOs.Game;

namespace GameStore.Services.Services;

public interface IGameService
{
    Task<GameFullDto> GetGameByAliasAsync(string alias);

    Task<IList<GameBriefDto>> GetAllGamesAsync();

    Task<GameFullDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);

    Task DeleteGameAsync(long gameId);

    Task<Tuple<byte[], string>> DownloadAsync(string gameAlias);
}