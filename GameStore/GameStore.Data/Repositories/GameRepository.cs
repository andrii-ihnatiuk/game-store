using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Models;
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

    public async Task<EntityFilteringResult<Game>> GetFilteredGamesAsync(GamesFilter filter)
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

        FilterByGenres(ref query, filter);
        FilterByPlatforms(ref query, filter);
        FilterByPublishers(ref query, filter);

        query = query.Distinct();
        ApplySorting(ref query, filter.Sort);

        var allData = await query.Select(i => i.LegacyId).ToListAsync();
        int total = allData.Count;
        IList<string> blacklist = allData.Where(i => i is not null).ToList()!;

        var games = await query.Take(filter.Limit).ToListAsync();
        return new EntityFilteringResult<Game>(games, total)
        {
            MongoBlacklist = blacklist,
        };
    }

    private static bool IsFilterSet(string? filterBy)
    {
        return !string.IsNullOrEmpty(filterBy);
    }

    private void FilterByGenres(ref IQueryable<Game> query, GamesFilter filter)
    {
        if (filter.Genres.Any() || filter.MongoCategories.Any())
        {
            query = query.Join(
                    Context.Set<GameGenre>(),
                    game => game.Id,
                    gg => gg.GameId,
                    (game, gameGenre) => new { game, gameGenre })
                .Where(q => filter.Genres.Contains(q.gameGenre.GenreId))
                .Select(q => q.game);
        }
    }

    private void FilterByPlatforms(ref IQueryable<Game> query, GamesFilter filter)
    {
        if (filter.Platforms.Any())
        {
            query = query.Join(
                    Context.Set<GamePlatform>(),
                    g => g.Id,
                    gp => gp.GameId,
                    (game, gamePlatform) => new { game, gamePlatform })
                .Where(q => filter.Platforms.Contains(q.gamePlatform.PlatformId))
                .Select(q => q.game);
        }
    }

    private static void FilterByPublishers(ref IQueryable<Game> query, GamesFilter filter)
    {
        if (filter.Publishers.Any() || filter.MongoSuppliers.Any())
        {
            query = query.Where(g => filter.Publishers.Contains((Guid)g.PublisherId!));
        }
    }

    private static DateTime GetLowerPublishDateLimit(GamesFilter filter)
    {
        var now = DateTime.UtcNow;
        return filter.DatePublishing switch
        {
            PublishDateOption.LastWeek => now.AddDays(-7),
            PublishDateOption.LastMonth => now.AddMonths(-1),
            PublishDateOption.LastYear => now.AddYears(-1),
            PublishDateOption.LastTwoYears => now.AddYears(-2),
            PublishDateOption.LastThreeYears => now.AddYears(-3),
            _ => throw new ArgumentOutOfRangeException(nameof(GamesFilterDto.DatePublishing)),
        };
    }

    private static void ApplySorting(ref IQueryable<Game> query, string? sorting)
    {
        if (string.IsNullOrEmpty(sorting))
        {
            return;
        }

        query = sorting switch
        {
            SortingOption.PriceAsc => query.OrderBy(g => g.Price),
            SortingOption.PriceDesc => query.OrderByDescending(g => g.Price),
            SortingOption.MostCommented => query.OrderByDescending(g => g.Comments.Count),
            SortingOption.MostPopular => query.OrderByDescending(g => g.PageViews),
            SortingOption.Newest => query.OrderByDescending(g => g.CreationDate),
            _ => throw new ArgumentOutOfRangeException(nameof(GamesFilterDto.Sort)),
        };
    }
}