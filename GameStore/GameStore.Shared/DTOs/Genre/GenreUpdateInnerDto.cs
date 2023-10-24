namespace GameStore.Shared.DTOs.Genre;

public class GenreUpdateInnerDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? ParentGenreId { get; set; }
}