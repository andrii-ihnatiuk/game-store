namespace GameStore.Data.Models;

public class GamesFilter
{
    public IList<Guid> Genres { get; set; } = new List<Guid>();

    public IList<Guid> Platforms { get; set; } = new List<Guid>();

    public IList<Guid> Publishers { get; set; } = new List<Guid>();

    public decimal? MaxPrice { get; set; }

    public decimal? MinPrice { get; set; }

    public string? DatePublishing { get; set; }

    public string? Name { get; set; }

    public string? Sort { get; set; }

    public int Page { get; set; }

    public int PageCount { get; set; }

    public string Trigger { get; set; }
}