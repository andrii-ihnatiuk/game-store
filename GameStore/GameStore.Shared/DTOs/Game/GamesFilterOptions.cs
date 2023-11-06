namespace GameStore.Shared.DTOs.Game;

public class GamesFilterOptions
{
    public IList<Guid> Genres { get; set; } = new List<Guid>();

    public IList<Guid> Platforms { get; set; } = new List<Guid>();

    public IList<Guid> Publishers { get; set; } = new List<Guid>();

    public decimal? MaxPrice { get; set; }

    public decimal? MinPrice { get; set; }

    public string? DatePublishing { get; set; }

    public string? Name { get; set; }
}