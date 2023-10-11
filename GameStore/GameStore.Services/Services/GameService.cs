using System.Text;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Shared.DTOs.Game;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GameFullDto> GetGameByAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == alias);
        return _mapper.Map<Game, GameFullDto>(game);
    }

    public async Task<IList<GameBriefDto>> GetAllGamesAsync()
    {
        var games = await _unitOfWork.Games.GetAsync(orderBy: q => q.OrderBy(g => g.Id));
        return _mapper.Map<IList<GameBriefDto>>(games);
    }

    public async Task<GameFullDto> AddGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);
        await ThrowIfGameAliasIsNotUnique(game.Alias);
        await ThrowIfForeignKeyConstraintViolationFor(game);
        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<Game, GameFullDto>(game);
    }

    public async Task UpdateGameAsync(GameUpdateDto dto)
    {
        var existingGame = await _unitOfWork.Games.GetByIdAsync(dto.GameId);
        if (existingGame.Alias != dto.Alias)
        {
            await ThrowIfGameAliasIsNotUnique(dto.Alias);
        }

        var updatedGame = _mapper.Map(dto, existingGame);
        await ThrowIfForeignKeyConstraintViolationFor(updatedGame);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGameAsync(long gameId)
    {
        await _unitOfWork.Games.DeleteAsync(gameId);
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

    private async Task ThrowIfForeignKeyConstraintViolationFor(Game game)
    {
        if (game.GenreId != null)
        {
            bool genreExists = await _unitOfWork.Genres.GetQueryable().AnyAsync(g => g.Id == game.GenreId);
            if (!genreExists)
            {
                throw new ForeignKeyException(onColumn: nameof(game.GenreId));
            }
        }

        if (game.PlatformId != null)
        {
            bool platformExists = await _unitOfWork.Platforms.GetQueryable().AnyAsync(p => p.Id == game.PlatformId);
            if (!platformExists)
            {
                throw new ForeignKeyException(onColumn: nameof(game.PlatformId));
            }
        }
    }

    private async Task ThrowIfGameAliasIsNotUnique(string alias)
    {
        bool aliasIsNotUnique = await _unitOfWork.Games.GetQueryable().AnyAsync(g => g.Alias == alias);
        if (aliasIsNotUnique)
        {
            throw new EntityAlreadyExistsException(nameof(Game.Alias), alias);
        }
    }
}