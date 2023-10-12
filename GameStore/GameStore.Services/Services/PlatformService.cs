﻿using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
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

    public async Task<PlatformFullDto> GetPlatformByIdAsync(long id)
    {
        var platform = await _unitOfWork.Platforms.GetOneAsync(
            p => p.Id == id,
            p => p.Include(nav => nav.Games));
        return _mapper.Map<PlatformFullDto>(platform);
    }

    public async Task<IList<PlatformBriefDto>> GetAllPlatformsAsync()
    {
        var platforms = await _unitOfWork.Platforms.GetAsync(orderBy: q => q.OrderBy(p => p.Id));
        return _mapper.Map<IList<PlatformBriefDto>>(platforms);
    }

    public async Task<PlatformFullDto> AddPlatformAsync(PlatformCreateDto dto)
    {
        var platform = _mapper.Map<Platform>(dto);
        await ThrowIfPlatformTypeIsNotUnique(platform.Type);
        await _unitOfWork.Platforms.AddAsync(platform);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<PlatformFullDto>(platform);
    }

    public async Task UpdatePlatformAsync(PlatformUpdateDto dto)
    {
        var existingPlatform = await _unitOfWork.Platforms.GetByIdAsync(dto.PlatformId);
        if (existingPlatform.Type != dto.Type)
        {
            await ThrowIfPlatformTypeIsNotUnique(dto.Type);
        }

        _mapper.Map(dto, existingPlatform);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeletePlatformAsync(long id)
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