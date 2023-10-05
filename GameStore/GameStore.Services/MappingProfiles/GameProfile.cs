using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs;

namespace GameStore.Services.MappingProfiles;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameCreateDto, Game>()
            .ForMember(
                dest => dest.Alias,
                opts => opts.MapFrom(src => ConstructAlias(src)));

        CreateMap<Game, GameViewDto>();
    }

    private static string ConstructAlias(GameCreateDto dto)
    {
        return string.IsNullOrEmpty(dto.Alias)
            ? string.Join('-', dto.Name.Split(' ')).ToLower()
            : dto.Alias;
    }
}