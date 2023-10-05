using GameStore.Data.Entities;

namespace GameStore.Data.Repositories;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    public GameRepository(GameStoreDbContext context)
        : base(context)
    {
    }
}