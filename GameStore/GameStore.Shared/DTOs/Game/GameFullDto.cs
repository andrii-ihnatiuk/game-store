﻿namespace GameStore.Shared.DTOs.Game;

public class GameFullDto
{
    public string Id { get; set; }

    public string Key { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Type { get; set; }

    public string? FileSize { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public ushort Discount { get; set; }

    public short UnitInStock { get; set; }

    public bool Discontinued { get; set; }

    public bool Deleted { get; set; }

    public string? PublisherId { get; set; }

    public DateTime? PublishDate { get; set; }
}