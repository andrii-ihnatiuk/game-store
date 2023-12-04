using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Application.Interfaces.Migration;

public interface IPublisherMigrationService : IOnUpdateMigrationService<PublisherUpdateDto>
{
}