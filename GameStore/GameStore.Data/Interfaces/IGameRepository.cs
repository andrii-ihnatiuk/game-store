using GameStore.Data.Entities;
using GameStore.Data.Models;

namespace GameStore.Data.Interfaces;

public interface IGameRepository : IGenericRepository<Game>
{
    Task<Tuple<IList<Game>, int>> GetFilteredGamesAsync(GamesFilter filter);
}