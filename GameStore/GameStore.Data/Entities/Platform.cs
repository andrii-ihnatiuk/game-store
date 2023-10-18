namespace GameStore.Data.Entities;

public class Platform
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public IList<GamePlatform> PlatformGames { get; set; } = new List<GamePlatform>();
}