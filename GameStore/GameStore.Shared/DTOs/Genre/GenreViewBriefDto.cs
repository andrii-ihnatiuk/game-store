namespace GameStore.Shared.DTOs.Genre;

public class GenreViewBriefDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long? ParentGenreId { get; set; }
}