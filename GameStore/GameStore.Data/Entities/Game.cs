﻿namespace GameStore.Data.Entities;

public class Game
{
    public long Id { get; set; }

    public string Alias { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
}