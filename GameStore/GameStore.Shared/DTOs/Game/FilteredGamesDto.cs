namespace GameStore.Shared.DTOs.Game;

public class FilteredGamesDto
{
    public FilteredGamesDto(IList<GameFullDto> games, int totalPages, int currentPage)
    {
        Games = games;
        TotalPages = totalPages;
        CurrentPage = currentPage;
    }

    public IList<GameFullDto> Games { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }
}