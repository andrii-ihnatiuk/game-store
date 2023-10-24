namespace GameStore.Shared.DTOs.Genre;

public class GenreCreateInnerDto
{
    public string Name { get; set; }

    public string? ParentGenreId { get; set; }
}