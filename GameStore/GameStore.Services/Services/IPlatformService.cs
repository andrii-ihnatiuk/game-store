using GameStore.Shared.DTOs.Platform;

namespace GameStore.Services.Services;

public interface IPlatformService
{
    Task<PlatformFullDto> GetPlatformByIdAsync(Guid id);

    Task<IList<PlatformBriefDto>> GetAllPlatformsAsync();

    Task<PlatformFullDto> AddPlatformAsync(PlatformCreateDto dto);

    Task UpdatePlatformAsync(PlatformUpdateDto dto);

    Task DeletePlatformAsync(long id);
}