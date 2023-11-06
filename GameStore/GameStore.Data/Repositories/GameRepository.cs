using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Game;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data.Repositories;

public class GameRepository : GenericRepository<Game>, IGameRepository
{
    public GameRepository(GameStoreDbContext context)
        : base(context)
    {
        Context = context;
    }

    private GameStoreDbContext Context { get; }

    public async Task<IList<Game>> GetFilteredGamesAsync(GamesFilterOptions filter)
    {
        var query = Context.Games.AsQueryable();

        query = filter.MaxPrice is null ? query : query.Where(g => g.Price <= filter.MaxPrice);
        query = filter.MinPrice is null ? query : query.Where(g => g.Price >= filter.MinPrice);

        if (IsFilterSet(filter.DatePublishing))
        {
            var dateLimit = GetLowerPublishDateLimit(filter);
            query = query.Where(g => g.PublishDate >= dateLimit);
        }

        if (IsFilterSet(filter.Name))
        {
            query = query.Where(g => EF.Functions.Like(g.Name, $"%{filter.Name}%"));
        }

        FilterByGenres(ref query, filter.Genres);
        FilterByPlatforms(ref query, filter.Platforms);
        FilterByPublishers(ref query, filter.Publishers);

        return await query.Distinct().ToListAsync();
    }

    private static bool IsFilterSet(string? filterBy)
    {
        return !string.IsNullOrEmpty(filterBy);
    }

    private void FilterByGenres(ref IQueryable<Game> query, ICollection<Guid> genres)
    {
        if (genres.Count > 0)
        {
            query = query.Join(
                    Context.Set<GameGenre>(),
                    game => game.Id,
                    gg => gg.GameId,
                    (game, gameGenre) => new { game, gameGenre })
                .Where(q => genres.Contains(q.gameGenre.GenreId))
                .Select(q => q.game);
        }
    }

    private void FilterByPlatforms(ref IQueryable<Game> query, ICollection<Guid> platforms)
    {
        if (platforms.Count > 0)
        {
            query = query.Join(
                    Context.Set<GamePlatform>(),
                    g => g.Id,
                    gp => gp.GameId,
                    (game, gamePlatform) => new { game, gamePlatform })
                .Where(q => platforms.Contains(q.gamePlatform.PlatformId))
                .Select(q => q.game);
        }
    }

    private static void FilterByPublishers(ref IQueryable<Game> query, ICollection<Guid> publishers)
    {
        if (publishers.Count > 0)
        {
            query = query.Where(g => publishers.Contains((Guid)g.PublisherId!));
        }
    }

    private static DateTime GetLowerPublishDateLimit(GamesFilterOptions filter)
    {
        var now = DateTime.UtcNow;
        return filter.DatePublishing switch
        {
            PublishDateOption.LastWeek => now.AddDays(-7),
            PublishDateOption.LastMonth => now.AddMonths(-1),
            PublishDateOption.LastYear => now.AddYears(-1),
            PublishDateOption.LastTwoYears => now.AddYears(-2),
            PublishDateOption.LastThreeYears => now.AddYears(-3),
            _ => throw new ArgumentOutOfRangeException(nameof(filter.DatePublishing)),
        };
    }
}