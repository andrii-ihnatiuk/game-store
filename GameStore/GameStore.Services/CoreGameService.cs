using System.Text;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CoreGameService : CoreServiceBase, ICoreGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoreGameService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GameFullDto> GetGameByAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == alias, noTracking: false);
        game.PageViews += 1;
        await _unitOfWork.SaveAsync(logChanges: false);
        return _mapper.Map<Game, GameFullDto>(game);
    }

    public async Task<GameFullDto> GetGameByIdAsync(string id)
    {
        var gameId = Guid.Parse(id);
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Id == gameId);
        return _mapper.Map<Game, GameFullDto>(game);
    }

    public async Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias)
    {
        var genresByGame = (await _unitOfWork.GamesGenres.GetAsync(
                predicate: gg => gg.Game.Alias == alias,
                include: q => q.Include(gg => gg.Genre)))
            .Select(gg => gg.Genre);
        return _mapper.Map<IList<GenreBriefDto>>(genresByGame);
    }

    public async Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias)
    {
        var platformsByGame = (await _unitOfWork.GamesPlatforms.GetAsync(
                predicate: gp => gp.Game.Alias == alias,
                include: q => q.Include(gp => gp.Platform)))
            .Select(gp => gp.Platform);
        return _mapper.Map<IList<PlatformBriefDto>>(platformsByGame);
    }

    public async Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Alias == alias,
            include: q => q.Include(g => g.Publisher));
        return _mapper.Map<PublisherBriefDto>(game.Publisher);
    }

    public async Task<EntityFilteringResult<GameFullDto>> GetAllGamesAsync(GamesFilter filter)
    {
        var filteringResult = await _unitOfWork.Games.GetFilteredGamesAsync(filter);
        return new EntityFilteringResult<GameFullDto>
        {
            Records = _mapper.Map<IList<GameFullDto>>(filteringResult.Records),
            TotalNoLimit = filteringResult.TotalNoLimit,
            MongoBlacklist = filteringResult.MongoBlacklist,
        };
    }

    public async Task<GameBriefDto> AddGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);
        await _unitOfWork.Games.ThrowIfGameAliasIsNotUnique(game.Alias);
        await ThrowIfForeignKeyConstraintViolation(game);
        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<GameBriefDto>(game);
    }

    public async Task UpdateGameAsync(GameUpdateDto dto)
    {
        var existingGame = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Id == Guid.Parse(dto.Game.Id),
            include: q =>
            {
                return q
                    .Include(g => g.GameGenres)
                    .Include(g => g.GamePlatforms);
            },
            noTracking: false);

        if (existingGame.Alias != dto.Game.Key)
        {
            await _unitOfWork.Games.ThrowIfGameAliasIsNotUnique(dto.Game.Key);
        }

        var updatedGame = _mapper.Map(dto, existingGame);

        await ThrowIfForeignKeyConstraintViolation(updatedGame);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGameAsync(string alias)
    {
        var gameToRemove = await _unitOfWork.Games.GetOneAsync(g => g.Alias == alias);
        await _unitOfWork.Games.DeleteAsync(gameToRemove.Id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Tuple<byte[], string>> DownloadAsync(string gameAlias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == gameAlias);
        string content = $"Game: {game.Name}\n\nDescription: {game.Description}";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string fileName = $"{game.Name}_{timestamp}.txt";
        return new Tuple<byte[], string>(bytes, fileName);
    }

    private async Task ThrowIfForeignKeyConstraintViolation(Game game)
    {
        await _unitOfWork.Genres.ThrowIfForeignKeyConstraintViolation(game.GameGenres);
        await _unitOfWork.Platforms.ThrowIfForeignKeyConstraintViolation(game.GamePlatforms);
        await _unitOfWork.Publishers.ThrowIfForeignKeyConstraintViolation(game);
    }
}