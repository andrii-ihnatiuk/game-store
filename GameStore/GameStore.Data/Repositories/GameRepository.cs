using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Data.Models;
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

    public async Task<Tuple<IList<Game>, int>> GetFilteredGamesAsync(GamesFilter filter)
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

        query = query.Distinct();
        ApplySorting(ref query, filter.Sort);
        (query, int totalPages) = await ApplyPagination(query, filter);

        var games = await query.ToListAsync();
        return new Tuple<IList<Game>, int>(games, totalPages);
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
            SortingOption.MostPopular => throw new NotImplementedException(),
            SortingOption.Newest => query.OrderByDescending(g => g.CreationDate),
            _ => throw new ArgumentOutOfRangeException(nameof(GamesFilterDto.Sort)),
        };
    }

    private static async Task<Tuple<IQueryable<Game>, int>> ApplyPagination(IQueryable<Game> query, GamesFilter filter)
    {
        int resultsCount = await query.CountAsync();
        int totalPages = (int)Math.Ceiling((double)resultsCount / filter.PageCount);
        int skipCount = filter.PageCount == int.MaxValue ? 0 : (filter.Page - 1) * filter.PageCount;

        query = query.Skip(skipCount).Take(filter.PageCount);
        return new Tuple<IQueryable<Game>, int>(query, totalPages);
    }
}