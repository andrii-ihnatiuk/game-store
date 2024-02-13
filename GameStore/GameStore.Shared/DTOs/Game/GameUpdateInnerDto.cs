namespace GameStore.Shared.DTOs.Game;

public class GameUpdateInnerDto
{
    public string Id { get; set; }

    public string Key { get; set; }

    public string Name { get; set; }

    public string? Type { get; set; }

    public string? FileSize { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public ushort Discount { get; set; }

    public short UnitInStock { get; set; }

    public bool Discontinued { get; set; }

    public DateTime? PublishDate { get; set; }
}