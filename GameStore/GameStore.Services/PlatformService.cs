using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Data.Interfaces;
using GameStore.Services.Extensions;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class PlatformService : MultiLingualEntityServiceBase<Platform, PlatformTranslation>, IPlatformService
{
    private readonly IUnitOfWork _unitOfWork;

    public PlatformService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(mapper)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PlatformFullDto> GetPlatformByIdAsync(Guid id, string culture)
    {
        var platform = await _unitOfWork.Platforms.GetOneAsync(
            predicate: p => p.Id == id,
            include: q => q.Include(p => p.Translations.Where(t => t.LanguageCode == culture)));
        return Mapper.MapWithTranslation<PlatformFullDto, PlatformTranslation>(platform, culture);
    }

    public async Task<IList<PlatformBriefDto>> GetAllPlatformsAsync(string culture)
    {
        var platforms = await _unitOfWork.Platforms.GetAsync(
            include: q => q.Include(p => p.Translations.Where(t => t.LanguageCode == culture)),
            orderBy: q => q.OrderBy(p => p.Id));
        return Mapper.MapWithTranslation<IList<PlatformBriefDto>, PlatformTranslation>(platforms, culture);
    }

    public async Task<IList<GameBriefDto>> GetGamesByPlatformAsync(Guid id, string culture)
    {
        var games = (await _unitOfWork.GamesPlatforms.GetAsync(
                predicate: gp => gp.PlatformId == id,
                include: q => q
                    .Include(gp => gp.Game)
                    .ThenInclude(g => g.Translations.Where(t => t.LanguageCode == culture))))
            .Select(gp => gp.Game);
        return Mapper.MapWithTranslation<IList<GameBriefDto>, GameTranslation>(games, culture);
    }

    public async Task<PlatformBriefDto> AddPlatformAsync(PlatformCreateDto dto)
    {
        var platform = Mapper.Map<Platform>(dto);
        await ThrowIfPlatformTypeIsNotUnique(platform.Type);
        await _unitOfWork.Platforms.AddAsync(platform);
        await _unitOfWork.SaveAsync();
        return Mapper.Map<PlatformBriefDto>(platform);
    }

    public async Task UpdatePlatformAsync(PlatformUpdateDto dto)
    {
        var existingPlatform = await _unitOfWork.Platforms.GetByIdAsync(dto.Platform.Id);
        if (existingPlatform.Type != dto.Platform.Type)
        {
            await ThrowIfPlatformTypeIsNotUnique(dto.Platform.Type);
        }

        UpdateMultiLingualEntity(existingPlatform, dto, dto.Culture);
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