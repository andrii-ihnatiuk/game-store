using GameStore.Shared.DTOs.Genre;

namespace GameStore.Application.Interfaces.Migration;

public interface IGenreMigrationService : IOnUpdateMigrationService<GenreUpdateDto>, IOnCreateMigrationService<GenreCreateDto>
{
}