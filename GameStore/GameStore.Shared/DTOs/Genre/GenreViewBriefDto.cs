﻿namespace GameStore.Shared.DTOs.Genre;

public class GenreViewBriefDto
{
    public long GenreId { get; set; }

    public string Name { get; set; }

    public long? ParentGenreId { get; set; }
}