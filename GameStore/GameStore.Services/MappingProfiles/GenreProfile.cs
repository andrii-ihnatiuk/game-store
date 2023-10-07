using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Services.MappingProfiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<GenreCreateDto, Genre>();

        CreateMap<Genre, GenreViewFullDto>();

        CreateMap<Genre, GenreViewBriefDto>();
    }
}