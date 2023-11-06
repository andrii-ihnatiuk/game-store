using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Game;

namespace GameStore.Data.Interfaces;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<IList<Game>> GetFilteredGamesAsync(GamesFilterOptions filter);
}