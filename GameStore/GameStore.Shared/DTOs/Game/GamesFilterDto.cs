using GameStore.Shared.Constants.Filter;

namespace GameStore.Shared.DTOs.Game;

public class GamesFilterDto
{
    public IList<string> Stores { get; set; } = new List<string> { StoreOption.GameStore, StoreOption.Northwind };

    public IList<string> Genres { get; set; } = new List<string>();

    public IList<Guid> Platforms { get; set; } = new List<Guid>();

    public IList<string> Publishers { get; set; } = new List<string>();

    public decimal? MaxPrice { get; set; }

    public decimal? MinPrice { get; set; }

    public string? DatePublishing { get; set; }

    public string? Name { get; set; }

    public string? Sort { get; set; }

    public int? Page { get; set; }

    public string? PageCount { get; set; }

    public string Trigger { get; set; } = FilterTrigger.Button;

    public static GamesFilterDto GameStoreDefaultFilter => new()
    {
        Stores = new List<string> { StoreOption.GameStore },
        PageCount = PaginationOption.All,
    };
}