using GameStore.Shared.DTOs.Game;

namespace GameStore.Application.Interfaces;

public interface IGameFacadeService
{
    Task<GameFullDto> GetGameByIdAsync(string id);

    Task<GameFullDto> GetGameByAliasAsync(string alias);

    Task<GameBriefDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);

    Task DeleteGameAsync(string alias);
}