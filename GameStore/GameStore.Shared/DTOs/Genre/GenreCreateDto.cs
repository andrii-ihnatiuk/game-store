namespace GameStore.Shared.DTOs.Genre;

public class GenreCreateDto
{
    public string Name { get; set; }

    public long? ParentGenreId { get; set; }
}