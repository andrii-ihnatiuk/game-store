namespace GameStore.Data.Entities;

public class Platform
{
    public long Id { get; set; }

    public string Type { get; set; }

    public IList<Game> Games { get; set; } = new List<Game>();
}