namespace GameStore.Shared.DTOs.Game;

public class GameFullDto
{
    public Guid Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public short UnitInStock { get; set; }

    public bool Discontinued { get; set; }
}