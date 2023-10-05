using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Shared.DTOs;

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
        var game = await _unitOfWork.Games.GetByIdAsync(alias);
        return _mapper.Map<Game, GameViewDto>(game);
    }

    public async Task<GameViewDto> AddGameAsync(GameCreateDto dto)
    {
        var game = _mapper.Map<Game>(dto);
        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<Game, GameViewDto>(game);
    }
}