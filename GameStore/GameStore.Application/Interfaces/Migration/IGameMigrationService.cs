using GameStore.Shared.DTOs.Game;

namespace GameStore.Application.Interfaces.Migration;

public interface IGameMigrationService : IOnUpdateMigrationService<GameUpdateDto>, IOnCreateMigrationService<GameCreateDto>
{
}