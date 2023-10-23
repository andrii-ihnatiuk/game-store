﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreInnerDto, Genre>();
        CreateMap<GenreCreateDto, Genre>()
            .IncludeMembers(g => g.Genre);

        CreateMap<Genre, GenreFullDto>();

        CreateMap<Genre, GenreBriefDto>();

        CreateMap<GenreUpdateDto, Genre>();
    }
}