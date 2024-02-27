using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Services.Interfaces;

public interface IPlatformService
{
    Task<PlatformFullDto> GetPlatformByIdAsync(Guid id, string culture);

    Task<IList<PlatformBriefDto>> GetAllPlatformsAsync(string culture);

    Task<IList<GameBriefDto>> GetGamesByPlatformAsync(Guid id, string culture);

    Task<PlatformBriefDto> AddPlatformAsync(PlatformCreateDto dto);

    Task UpdatePlatformAsync(PlatformUpdateDto dto);

    Task DeletePlatformAsync(Guid id);
}