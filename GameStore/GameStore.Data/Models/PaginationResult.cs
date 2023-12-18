using GameStore.Data.Entities;

namespace GameStore.Data.Models;

public struct PaginationResult
{
    public PaginationResult(IQueryable<Game> query, int totalPages, IList<string> legacyIds)
    {
        Query = query;
        TotalPages = totalPages;
        LegacyIds = legacyIds;
    }

    public IQueryable<Game> Query { get; set; }

    public int TotalPages { get; set; }

    public IList<string> LegacyIds { get; set; }
}