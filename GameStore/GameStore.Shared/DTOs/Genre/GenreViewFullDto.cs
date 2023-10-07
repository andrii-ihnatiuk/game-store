﻿namespace GameStore.Shared.DTOs.Genre;

public class GenreViewFullDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public long? ParentGenreId { get; set; }

    public IList<GenreViewBriefDto> SubGenres { get; set; } = new List<GenreViewBriefDto>();
}