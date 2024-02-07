using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Image;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services.Interfaces;

public interface ICoreGameService : IGameService
{
    Task<IList<ImageBriefDto>> GetImagesByGameAliasAsync(string gameAlias);

    Task<GameBriefDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);
}