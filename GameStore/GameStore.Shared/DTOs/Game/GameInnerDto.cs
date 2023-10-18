namespace GameStore.Shared.DTOs.Game;

public class GameInnerDto
{
    public string? Key { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public short UnitInStock { get; set; }

    public ushort Discontinued { get; set; }
}