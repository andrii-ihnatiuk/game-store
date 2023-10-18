namespace GameStore.Shared.DTOs.Genre;

public class GenreUpdateDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}