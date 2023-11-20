namespace GameStore.Shared.DTOs.Genre;

public class GenreBriefDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string? ParentGenreId { get; set; }
}