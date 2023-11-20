using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class GameFacadeService : IGameFacadeService
{
    private readonly IServiceResolver _serviceResolver;

    public GameFacadeService(IServiceResolver serviceResolver)
    {
        _serviceResolver = serviceResolver;
    }

    public Task<GameFullDto> GetGameByIdAsync(string id)
    {
        var gameService = _serviceResolver.ResolveForEntityId<IGameService>(id);
        return gameService.GetGameByIdAsync(id);
    }

    public Task<GameFullDto> GetGameByAliasAsync(string alias)
    {
        var gameService = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);
        return gameService.GetGameByAliasAsync(alias);
    }
}