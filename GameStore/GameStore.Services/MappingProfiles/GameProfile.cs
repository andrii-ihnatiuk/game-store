using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Game;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
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

        CreateMap<Game, GameBriefDto>()
            .ForMember(
                dest => dest.GameId,
                opts => opts.MapFrom(src => src.Id));
        CreateMap<Game, GameFullDto>()
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