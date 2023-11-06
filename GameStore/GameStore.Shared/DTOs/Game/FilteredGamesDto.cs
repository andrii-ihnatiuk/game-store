namespace GameStore.Shared.DTOs.Game;

public class FilteredGamesDto
{
    public IList<GameBriefDto> Games { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }
}