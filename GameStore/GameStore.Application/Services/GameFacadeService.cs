using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Migration;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Interfaces.Services;

namespace GameStore.Application.Services;

public class GameFacadeService : IGameFacadeService
{
    private readonly IServiceResolver _serviceResolver;
    private readonly IGameMigrationService _migrationService;

    public GameFacadeService(IServiceResolver serviceResolver, IGameMigrationService migrationService)
    {
        _serviceResolver = serviceResolver;
        _migrationService = migrationService;
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

    public async Task<GameBriefDto> AddGameAsync(GameCreateDto dto)
    {
        await _migrationService.MigrateOnCreateAsync(dto);
        var service = _serviceResolver.ResolveAll<ICoreGameService>().Single();
        return await service.AddGameAsync(dto);
    }

    public async Task UpdateGameAsync(GameUpdateDto dto)
    {
        await _migrationService.MigrateOnUpdateAsync(dto);
        var service = _serviceResolver.ResolveAll<ICoreGameService>().Single();
        await service.UpdateGameAsync(dto);
    }

    public Task DeleteGameAsync(string alias)
    {
        var service = _serviceResolver.ResolveForEntityAlias<ICoreGameService>(alias);
        return service.DeleteGameAsync(alias);
    }
}