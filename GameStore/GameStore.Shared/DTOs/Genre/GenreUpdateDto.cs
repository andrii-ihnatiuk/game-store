namespace GameStore.Shared.DTOs.Genre;

public class GenreUpdateDto
{
    public string Culture { get; set; }

    public GenreUpdateInnerDto Genre { get; set; }
}