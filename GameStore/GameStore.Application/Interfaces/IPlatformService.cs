using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Application.Interfaces;

public interface IPlatformService
{
    Task<PlatformFullDto> GetPlatformByIdAsync(Guid id);

    Task<IList<PlatformBriefDto>> GetAllPlatformsAsync();

    Task<IList<GameBriefDto>> GetGamesByPlatformAsync(Guid id);

    Task<PlatformBriefDto> AddPlatformAsync(PlatformCreateDto dto);

    Task UpdatePlatformAsync(PlatformUpdateDto dto);

    Task DeletePlatformAsync(Guid id);
}