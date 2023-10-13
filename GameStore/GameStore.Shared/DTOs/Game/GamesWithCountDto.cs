namespace GameStore.Shared.DTOs.Game;

public class GamesWithCountDto
{
    public int Count { get; set; }

    public IList<GameBriefDto> Games { get; set; }
}