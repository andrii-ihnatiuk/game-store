using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Game;

namespace GameStore.Services.MappingProfiles;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameCreateDto, Game>()
            .ForMember(
                dest => dest.Alias,
                opts => opts.MapFrom(src => ConstructAlias(src)));

        CreateMap<GameUpdateDto, Game>()
            .ForMember(
                dest => dest.Id,
                opts => opts.MapFrom(dest => dest.GameId));

        CreateMap<Game, GameViewBriefDto>()
            .ForMember(
                dest => dest.GameId,
                opts => opts.MapFrom(src => src.Id));
        CreateMap<Game, GameViewFullDto>()
            .ForMember(
                dest => dest.GameId,
                opts => opts.MapFrom(src => src.Id));
    }

    private static string ConstructAlias(GameCreateDto dto)
    {
        return string.IsNullOrEmpty(dto.Alias)
            ? string.Join('-', dto.Name.Split(' ')).ToLower()
            : dto.Alias;
    }
}