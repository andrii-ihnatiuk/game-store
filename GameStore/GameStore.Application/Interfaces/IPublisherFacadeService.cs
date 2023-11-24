using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Application.Interfaces;

public interface IPublisherFacadeService
{
    Task<PublisherFullDto> GetPublisherByNameAsync(string companyName);

    Task<IList<PublisherBriefDto>> GetAllPublishersAsync();

    Task<IList<GameBriefDto>> GetGamesByPublisherNameAsync(string companyName);

    Task UpdatePublisherAsync(PublisherUpdateDto dto);

    Task DeletePublisherAsync(string id);
}