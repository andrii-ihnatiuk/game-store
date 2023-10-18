namespace GameStore.Data.Entities;

public class Genre
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }

    public IList<Genre> SubGenres { get; set; } = new List<Genre>();

    public IList<GameGenre> GenreGames { get; set; } = new List<GameGenre>();
}