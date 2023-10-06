using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Shared.DTOs;
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

    public async Task<GameViewDto> GetGameByAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.FirstOrDefaultAsync(g => g.Alias == alias);
        return game is null ? throw new EntityNotFoundException(entityId: alias) : _mapper.Map<Game, GameViewDto>(game);
    }

    public async Task<GameViewDto> AddGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);

        var gameExists = await _unitOfWork.Games.GetQueryable().AnyAsync(g => g.Alias == game.Alias);
        if (gameExists)
        {
            throw new EntityAlreadyExistsException(nameof(game.Alias), game.Alias);
        }

        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<Game, GameViewDto>(game);
    }

    public async Task UpdateGameAsync(GameCreateDto dto)
    {
        var existingGame = await _unitOfWork.Games.GetByIdAsync(dto.GameId) ?? throw new EntityNotFoundException(entityId: dto.GameId);
        _mapper.Map(dto, existingGame);
        await _unitOfWork.SaveAsync();
    }
}