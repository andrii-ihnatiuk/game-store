using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Shared.Interfaces.Services;

public interface IPublisherService : IResolvableByEntityStorage
{
    Task<PublisherFullDto> GetPublisherByIdAsync(string id, string culture);

    Task<IList<PublisherBriefDto>> GetAllPublishersAsync(string culture);

    Task<IList<GameBriefDto>> GetGamesByPublisherIdAsync(string id, string culture);

    Task DeletePublisherAsync(string id);
}