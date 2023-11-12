using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Application.Interfaces;

public interface IPublisherService
{
    Task<PublisherFullDto> GetPublisherByNameAsync(string companyName);

    Task<IList<PublisherBriefDto>> GetAllPublishersAsync();

    Task<IList<GameBriefDto>> GetGamesByPublisherNameAsync(string companyName);

    Task<PublisherBriefDto> AddPublisherAsync(PublisherCreateDto dto);

    Task UpdatePublisherAsync(PublisherUpdateDto dto);

    Task DeletePublisherAsync(Guid id);
}