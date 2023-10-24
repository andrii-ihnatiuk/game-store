using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Platform;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.Services;

public class PlatformService : IPlatformService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlatformService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlatformFullDto> GetPlatformByIdAsync(Guid id)
    {
        var platform = await _unitOfWork.Platforms.GetOneAsync(
            p => p.Id == id);
        return _mapper.Map<PlatformFullDto>(platform);
    }

    public async Task<IList<PlatformBriefDto>> GetAllPlatformsAsync()
    {
        var platforms = await _unitOfWork.Platforms.GetAsync(orderBy: q => q.OrderBy(p => p.Id));
        return _mapper.Map<IList<PlatformBriefDto>>(platforms);
    }

    public async Task<IList<GameBriefDto>> GetGamesByPlatformAsync(Guid id)
    {
        var games = (await _unitOfWork.GamesPlatforms.GetAsync(
                predicate: gp => gp.PlatformId == id,
                include: q => q.Include(gp => gp.Game)))
            .Select(gp => gp.Game);
        return _mapper.Map<IList<GameBriefDto>>(games);
    }

    public async Task<PlatformBriefDto> AddPlatformAsync(PlatformCreateDto dto)
    {
        var platform = _mapper.Map<Platform>(dto);
        await ThrowIfPlatformTypeIsNotUnique(platform.Type);
        await _unitOfWork.Platforms.AddAsync(platform);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<PlatformBriefDto>(platform);
    }

    public async Task UpdatePlatformAsync(PlatformUpdateDto dto)
    {
        var existingPlatform = await _unitOfWork.Platforms.GetByIdAsync(dto.Platform.Id);
        if (existingPlatform.Type != dto.Platform.Type)
        {
            await ThrowIfPlatformTypeIsNotUnique(dto.Platform.Type);
        }

        _mapper.Map(dto, existingPlatform);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeletePlatformAsync(Guid id)
    {
        await _unitOfWork.Platforms.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }

    private async Task ThrowIfPlatformTypeIsNotUnique(string type)
    {
        bool typeIsNotUnique = await _unitOfWork.Platforms.ExistsAsync(p => p.Type == type);
        if (typeIsNotUnique)
        {
            throw new EntityAlreadyExistsException(nameof(Platform.Type), type);
        }
    }
}