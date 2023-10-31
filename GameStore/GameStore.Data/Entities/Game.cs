namespace GameStore.Data.Entities;

public class Game
{
    public Guid Id { get; set; }

    public string Alias { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public short UnitInStock { get; set; }

    public bool Discontinued { get; set; }

    public Guid? PublisherId { get; set; }

    public Publisher? Publisher { get; set; }

    public IList<GameGenre> GameGenres { get; set; } = new List<GameGenre>();

    public IList<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();

    public IList<Comment> Comments { get; set; } = new List<Comment>();
}