using GameStore.Shared.DTOs.Platform;

namespace GameStore.Services.Interfaces;

public interface IPlatformService
{
    Task<PlatformFullDto> GetPlatformByIdAsync(Guid id);

    Task<IList<PlatformBriefDto>> GetAllPlatformsAsync();

    Task<PlatformBriefDto> AddPlatformAsync(PlatformCreateDto dto);

    Task UpdatePlatformAsync(PlatformUpdateDto dto);

    Task DeletePlatformAsync(Guid id);
}