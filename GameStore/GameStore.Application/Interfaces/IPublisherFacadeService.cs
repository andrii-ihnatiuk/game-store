using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Application.Interfaces;

public interface IPublisherFacadeService
{
    Task<PublisherFullDto> GetPublisherByIdAsync(string id, string culture);

    Task<IList<PublisherBriefDto>> GetAllPublishersAsync(string culture);

    Task<IList<GameBriefDto>> GetGamesByPublisherIdAsync(string id, string culture);

    Task<PublisherBriefDto> AddPublisherAsync(PublisherCreateDto dto);

    Task UpdatePublisherAsync(PublisherUpdateDto dto);

    Task DeletePublisherAsync(string id);
}