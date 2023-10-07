using System.Text;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Exceptions;
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

    public async Task<GameViewFullDto> GetGameByAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.FirstOrDefaultAsync(g => g.Alias == alias);
        return game is null ? throw new EntityNotFoundException(entityId: alias) : _mapper.Map<Game, GameViewFullDto>(game);
    }

    public async Task<IList<GameViewBriefDto>> GetAllGamesAsync()
    {
        var games = await _unitOfWork.Games.GetAsync(orderBy: q => q.OrderBy(g => g.Id));
        var gamesDto = _mapper.Map<IList<Game>, IList<GameViewBriefDto>>(games);
        return gamesDto;
    }

    public async Task<GameViewFullDto> AddGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);

        var gameExists = await _unitOfWork.Games.GetQueryable().AnyAsync(g => g.Alias == game.Alias);
        if (gameExists)
        {
            throw new EntityAlreadyExistsException(nameof(game.Alias), game.Alias);
        }

        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<Game, GameViewFullDto>(game);
    }

    public async Task UpdateGameAsync(GameUpdateDto dto)
    {
        var existingGame = await _unitOfWork.Games.GetByIdAsync(dto.GameId)
                           ?? throw new EntityNotFoundException(entityId: dto.GameId);
        _mapper.Map(dto, existingGame);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGameAsync(long gameId)
    {
        await _unitOfWork.Games.DeleteAsync(gameId);
        await _unitOfWork.SaveAsync();
    }

    public async Task<Tuple<byte[], string>> DownloadAsync(string gameAlias)
    {
        var game = await _unitOfWork.Games.FirstOrDefaultAsync(g => g.Alias == gameAlias)
                   ?? throw new EntityNotFoundException(entityId: gameAlias);
        string content = $"Game: {game.Name}\n\nDescription: {game.Description}";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string fileName = $"{game.Name}_{timestamp}.txt";
        return new Tuple<byte[], string>(bytes, fileName);
    }
}