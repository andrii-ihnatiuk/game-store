namespace GameStore.Shared.DTOs.Genre;

public class GenreInnerDto
{
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; }
}