using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.Exceptions;
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

    public async Task<PlatformViewFullDto> GetPlatformByIdAsync(long id)
    {
        var platform = await _unitOfWork.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        return platform is null ? throw new EntityNotFoundException() : _mapper.Map<PlatformViewFullDto>(platform);
    }

    public async Task<IList<PlatformViewBriefDto>> GetAllPlatformsAsync()
    {
        var platforms = await _unitOfWork.Platforms.GetAsync(orderBy: q => q.OrderBy(p => p.Id));
        var platformsDto = _mapper.Map<IList<Platform>, IList<PlatformViewBriefDto>>(platforms);
        return platformsDto;
    }

    public async Task<PlatformViewFullDto> AddPlatformAsync(PlatformCreateDto dto)
    {
        var platform = _mapper.Map<Platform>(dto);

        var platformExists = await _unitOfWork.Platforms.GetQueryable().AnyAsync(p => p.Type == platform.Type);
        if (platformExists)
        {
            throw new EntityAlreadyExistsException(nameof(platform.Type), platform.Type);
        }

        await _unitOfWork.Platforms.AddAsync(platform);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<PlatformViewFullDto>(platform);
    }

    public async Task UpdatePlatformAsync(PlatformUpdateDto dto)
    {
        var existingPlatform = await _unitOfWork.Platforms.GetByIdAsync(dto.PlatformId)
                            ?? throw new EntityNotFoundException(entityId: dto.PlatformId);
        _mapper.Map(dto, existingPlatform);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeletePlatformAsync(long id)
    {
        await _unitOfWork.Platforms.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }
}