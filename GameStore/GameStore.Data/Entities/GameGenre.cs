namespace GameStore.Data.Entities;

public class GameGenre
{
    public Guid GameId { get; set; }

    public Guid GenreId { get; set; }

    public Game Game { get; set; }

    public Genre Genre { get; set; }
}