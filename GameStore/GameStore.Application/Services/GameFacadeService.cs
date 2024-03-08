using AutoMapper;
using GameStore.Application.Interfaces;
using GameStore.Application.Interfaces.Migration;
using GameStore.Application.Interfaces.Util;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.Constants.Filter;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Image;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using GameStore.Shared.Models;

namespace GameStore.Application.Services;

public class GameFacadeService : IGameFacadeService
{
    private readonly IEntityServiceResolver _serviceResolver;
    private readonly IGameMigrationService _migrationService;
    private readonly IMapper _mapper;

    public GameFacadeService(IEntityServiceResolver serviceResolver, IGameMigrationService migrationService, IMapper mapper)
    {
        _serviceResolver = serviceResolver;
        _migrationService = migrationService;
        _mapper = mapper;
    }

    public async Task<FilteredGamesDto> GetFilteredGamesAsync(GamesFilterDto filterDto, string culture, bool showDeleted = false)
    {
        var filter = _mapper.Map<GamesFilter>(filterDto);
        filter.ShowDeleted = showDeleted;
        filter.Culture = culture;
        filter.ResetPageIfTriggeredNotByPagination();

        var coreService = _serviceResolver.ResolveForEntityStorage<IGameService>(EntityStorage.SqlServer);
        var coreResult = await coreService.GetFilteredGamesAsync(filter);

        filter.Blacklist = coreResult.MongoBlacklist;
        var mongoService = _serviceResolver.ResolveForEntityStorage<IGameService>(EntityStorage.MongoDb);
        var mongoResult = await mongoService.GetFilteredGamesAsync(filter);

        IList<GameFullDto> games = coreResult.Records.Concat(mongoResult.Records).ToList();
        ApplySorting(ref games, filter.Sort);
        int totalPages = ApplyPagination(ref games, filter, coreResult.TotalNoLimit + mongoResult.TotalNoLimit);

        return new FilteredGamesDto(games, totalPages, currentPage: filter.Page);
    }

    public Task<GameFullDto> GetGameByIdAsync(string id, string culture)
    {
        var gameService = _serviceResolver.ResolveForEntityId<IGameService>(id);
        return gameService.GetGameByIdAsync(id, culture);
    }

    public Task<GameFullDto> GetGameByAliasAsync(string alias, string culture)
    {
        var gameService = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);
        return gameService.GetGameByAliasAsync(alias, culture);
    }

    public Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias)
    {
        var gameService = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);
        return gameService.GetGenresByGameAliasAsync(alias);
    }

    public Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias)
    {
        var gameService = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);
        return gameService.GetPlatformsByGameAliasAsync(alias);
    }

    public Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias)
    {
        var gameService = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);
        return gameService.GetPublisherByGameAliasAsync(alias);
    }

    public async Task<IList<ImageBriefDto>> GetImagesByGameAliasAsync(string alias)
    {
        var service = _serviceResolver.ResolveAll<ICoreGameService>().Single();
        return await service.GetImagesByGameAliasAsync(alias);
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
        var service = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);
        return service.DeleteGameAsync(alias);
    }

    public Task<Tuple<byte[], string>> DownloadAsync(string gameAlias)
    {
        var service = _serviceResolver.ResolveForEntityAlias<IGameService>(gameAlias);
        return service.DownloadAsync(gameAlias);
    }

    private static void ApplySorting(ref IList<GameFullDto> records, string? sorting)
    {
        if (string.IsNullOrEmpty(sorting))
        {
            return;
        }

        records = sorting switch
        {
            SortingOption.PriceAsc => records.OrderBy(g => g.Price).ToList(),
            SortingOption.PriceDesc => records.OrderByDescending(g => g.Price).ToList(),
            SortingOption.MostCommented => records,
            SortingOption.MostPopular => records,
            SortingOption.Newest => records,
            _ => throw new ArgumentOutOfRangeException(nameof(GamesFilterDto.Sort)),
        };
    }

    private static int ApplyPagination(ref IList<GameFullDto> records, GamesFilter filter, int totalNoLimit)
    {
        int totalPages = (int)Math.Ceiling((double)totalNoLimit / filter.PageCount);
        int skipCount = filter.PageCount == int.MaxValue ? 0 : (filter.Page - 1) * filter.PageCount;
        records = records.Skip(skipCount).Take(filter.PageCount).ToList();
        return totalPages;
    }
}