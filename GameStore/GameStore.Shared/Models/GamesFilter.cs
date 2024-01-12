using GameStore.Shared.Constants.Filter;

namespace GameStore.Shared.Models;

public class GamesFilter
{
    public IList<string> Stores { get; set; }

    public IList<Guid> Genres { get; set; } = new List<Guid>();

    public IList<string> MongoCategories { get; set; } = new List<string>();

    public IList<Guid> Platforms { get; set; } = new List<Guid>();

    public IList<Guid> Publishers { get; set; } = new List<Guid>();

    public IList<string> MongoSuppliers { get; set; } = new List<string>();

    public decimal? MaxPrice { get; set; }

    public decimal? MinPrice { get; set; }

    public string? DatePublishing { get; set; }

    public string? Name { get; set; }

    public string? Sort { get; set; }

    public int Page { get; set; }

    public int PageCount { get; set; }

    public string Trigger { get; set; }

    public IList<string> Blacklist { get; set; } = new List<string>();

    public int Limit { get; set; } = int.MaxValue;

    public bool ShowDeleted { get; set; }

    public void ResetPageIfTriggeredNotByPagination()
    {
        if (Trigger != FilterTrigger.PageChange)
        {
            Page = 1;
        }
    }
}