using GameStore.Data.Entities;
using GameStore.Shared.Models;

namespace GameStore.Data.Interfaces;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<EntityFilteringResult<Game>> GetFilteredGamesAsync(GamesFilter filter);
}