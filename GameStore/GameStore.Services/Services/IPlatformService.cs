using GameStore.Shared.DTOs.Platform;

namespace GameStore.Services.Services;

public interface IPlatformService
{
    Task<PlatformViewFullDto> GetPlatformByIdAsync(long id);

    Task<IList<PlatformViewBriefDto>> GetAllPlatformsAsync();

    Task<PlatformViewFullDto> AddPlatformAsync(PlatformCreateDto dto);

    Task UpdatePlatformAsync(PlatformUpdateDto dto);

    Task DeletePlatformAsync(long id);
}