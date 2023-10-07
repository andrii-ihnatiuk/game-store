namespace GameStore.Data.Entities;

public class Game
{
    public long Id { get; set; }

    public string Alias { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public long? GenreId { get; set; }

    public long? PlatformId { get; set; }

    public Genre? Genre { get; set; }

    public Platform? Platform { get; set; }
}