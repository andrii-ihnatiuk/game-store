namespace GameStore.Data.Entities;

public class Genre
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long? ParentGenreId { get; set; }

    public IList<Genre> SubGenres { get; set; } = new List<Genre>();

    public IList<Game> Games { get; set; } = new List<Game>();
}