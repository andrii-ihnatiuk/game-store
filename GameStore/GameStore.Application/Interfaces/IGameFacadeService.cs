using GameStore.Shared.DTOs.Game;

namespace GameStore.Application.Interfaces;

public interface IGameFacadeService
{
    Task<GameFullDto> GetGameByIdAsync(string id);

    Task<GameFullDto> GetGameByAliasAsync(string alias);
}