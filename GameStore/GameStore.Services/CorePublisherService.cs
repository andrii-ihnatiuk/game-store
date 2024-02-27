using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Services.Extensions;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CorePublisherService : MultiLingualEntityServiceBase<Publisher, PublisherTranslation>, ICorePublisherService
{
    private readonly IUnitOfWork _unitOfWork;

    public CorePublisherService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(mapper)
    {
        _unitOfWork = unitOfWork;
    }

    public EntityStorage EntityStorage => EntityStorage.SqlServer;

    public async Task<PublisherFullDto> GetPublisherByIdAsync(string id, string culture)
    {
        var publisher = await _unitOfWork.Publishers.GetOneAsync(
            predicate: p => p.Id == Guid.Parse(id),
            include: q => q.Include(p => p.Translations.Where(t => t.LanguageCode == culture)));
        return Mapper.MapWithTranslation<PublisherFullDto, PublisherTranslation>(publisher, culture);
    }

    public async Task<IList<PublisherBriefDto>> GetAllPublishersAsync(string culture)
    {
        var publishers = await _unitOfWork.Publishers.GetAsync(
            include: q => q.Include(p => p.Translations.Where(t => t.LanguageCode == culture)));
        return Mapper.MapWithTranslation<IList<PublisherBriefDto>, PublisherTranslation>(publishers, culture);
    }

    public async Task<IList<GameBriefDto>> GetGamesByPublisherIdAsync(string id, string culture)
    {
        var gamesByPublisher = await _unitOfWork.Games.GetAsync(
            predicate: g => g.Publisher.Id == Guid.Parse(id),
            include: q => q.Include(g => g.Translations.Where(t => t.LanguageCode == culture)));
        return Mapper.MapWithTranslation<IList<GameBriefDto>, GameTranslation>(gamesByPublisher, culture);
    }

    public async Task<PublisherBriefDto> AddPublisherAsync(PublisherCreateDto dto)
    {
        await _unitOfWork.Publishers.ThrowIfCompanyNameIsNotUnique(dto.Publisher.CompanyName);
        var publisher = Mapper.Map<Publisher>(dto);
        await _unitOfWork.Publishers.AddAsync(publisher);
        await _unitOfWork.SaveAsync();
        return Mapper.Map<PublisherBriefDto>(publisher);
    }

    public async Task UpdatePublisherAsync(PublisherUpdateDto dto)
    {
        var existingPublisher = await _unitOfWork.Publishers.GetOneAsync(
            predicate: p => p.Id == Guid.Parse(dto.Publisher.Id),
            include: q => q.Include(p => p.Translations.Where(t => t.LanguageCode == dto.Culture)),
            noTracking: false);

        if (existingPublisher.CompanyName != dto.Publisher.CompanyName)
        {
            await _unitOfWork.Publishers.ThrowIfCompanyNameIsNotUnique(dto.Publisher.CompanyName);
        }

        UpdateMultiLingualEntity(existingPublisher, dto, dto.Culture);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeletePublisherAsync(string id)
    {
        var publisherToRemove = await _unitOfWork.Publishers.GetOneAsync(g => g.Id == Guid.Parse(id));
        await _unitOfWork.Publishers.DeleteAsync(publisherToRemove.Id);
        await _unitOfWork.SaveAsync();
    }
}