﻿using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Services.Services;

public class PublisherService : IPublisherService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PublisherService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PublisherFullDto> GetPublisherByNameAsync(string companyName)
    {
        var publisher = await _unitOfWork.Publishers.GetOneAsync(
            p => p.CompanyName == companyName);
        return _mapper.Map<PublisherFullDto>(publisher);
    }

    public async Task<IList<PublisherBriefDto>> GetAllPublishersAsync()
    {
        var publishers = await _unitOfWork.Publishers.GetAsync();
        return _mapper.Map<IList<PublisherBriefDto>>(publishers);
    }

    public async Task<IList<GameBriefDto>> GetGamesByPublisherNameAsync(string companyName)
    {
        var gamesByPublisher = await _unitOfWork.Games.GetAsync(
            predicate: g => g.Publisher.CompanyName == companyName);
        return _mapper.Map<IList<GameBriefDto>>(gamesByPublisher);
    }

    public async Task<PublisherBriefDto> AddPublisherAsync(PublisherCreateDto dto)
    {
        var publisher = _mapper.Map<Publisher>(dto);
        await _unitOfWork.Publishers.AddAsync(publisher);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<PublisherBriefDto>(publisher);
    }

    public async Task DeletePublisherAsync(Guid id)
    {
        await _unitOfWork.Publishers.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }
}