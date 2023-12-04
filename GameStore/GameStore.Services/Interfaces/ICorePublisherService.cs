using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Services.Interfaces;

public interface ICorePublisherService : IPublisherService
{
    Task<PublisherBriefDto> AddPublisherAsync(PublisherCreateDto dto);

    Task UpdatePublisherAsync(PublisherUpdateDto dto);
}