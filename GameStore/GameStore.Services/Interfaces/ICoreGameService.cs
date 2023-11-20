using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services.Interfaces;

public interface ICoreGameService : IGameService
{
    Task<GameBriefDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);

    Task DeleteGameAsync(string alias);
}