using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Services.Extensions;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CoreGenreService : MultiLingualEntityServiceBase<Genre, GenreTranslation>, ICoreGenreService
{
    private readonly IUnitOfWork _unitOfWork;

    public CoreGenreService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(mapper)
    {
        _unitOfWork = unitOfWork;
    }

    public EntityStorage EntityStorage => EntityStorage.SqlServer;

    public async Task<GenreFullDto> GetGenreByIdAsync(string id, string culture)
    {
        var genreId = Guid.Parse(id);
        var genre = await _unitOfWork.Genres.GetOneAsync(
            g => g.Id == genreId,
            g => g
                .Include(g => g.Translations.Where(t => t.LanguageCode == culture))
                .Include(nav => nav.SubGenres));
        return Mapper.MapWithTranslation<GenreFullDto, GenreTranslation>(genre, culture);
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync(string culture)
    {
        var genres = await _unitOfWork.Genres.GetAsync(
            orderBy: q => q.OrderBy(g => g.Id),
            include: q => q.Include(g => g.Translations.Where(t => t.LanguageCode == culture)));
        return Mapper.MapWithTranslation<IList<GenreBriefDto>, GenreTranslation>(genres, culture);
    }

    public async Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId, string culture)
    {
        var genreId = Guid.Parse(parentId);
        var subgenres = await _unitOfWork.Genres.GetAsync(
            predicate: g => g.ParentGenreId == genreId,
            include: q => q.Include(g => g.Translations.Where(t => t.LanguageCode == culture)));
        return Mapper.MapWithTranslation<IList<GenreBriefDto>, GenreTranslation>(subgenres, culture);
    }

    public async Task<IList<GameBriefDto>> GetGamesByGenreIdAsync(string id, string culture)
    {
        var genreId = Guid.Parse(id);
        var games = (await _unitOfWork.GamesGenres.GetAsync(
                predicate: gg => gg.GenreId == genreId,
                include: q => q
                    .Include(gg => gg.Game)
                    .ThenInclude(g => g.Translations.Where(t => t.LanguageCode == culture))))
            .Select(gg => gg.Game);
        return Mapper.MapWithTranslation<IList<GameBriefDto>, GameTranslation>(games, culture);
    }

    public async Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto)
    {
        var genre = Mapper.Map<Genre>(dto);
        await _unitOfWork.Genres.ThrowIfGenreNameIsNotUnique(genre.Name);
        await _unitOfWork.Genres.ThrowIfForeignKeyConstraintViolation(genre);
        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveAsync();
        return Mapper.Map<GenreBriefDto>(genre);
    }

    public async Task UpdateGenreAsync(GenreUpdateDto dto)
    {
        var existingGenre = await _unitOfWork.Genres.GetOneAsync(
            predicate: g => g.Id == Guid.Parse(dto.Genre.Id),
            include: q => q.Include(g => g.Translations.Where(t => t.LanguageCode == dto.Culture)),
            noTracking: false);

        if (existingGenre.Name != dto.Genre.Name)
        {
            await _unitOfWork.Genres.ThrowIfGenreNameIsNotUnique(dto.Genre.Name);
        }

        UpdateMultiLingualEntity(existingGenre, dto, dto.Culture);

        await _unitOfWork.Genres.ThrowIfForeignKeyConstraintViolation(existingGenre);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGenreAsync(string genreId)
    {
        var genreToRemove = await _unitOfWork.Genres.GetOneAsync(g => g.Id == Guid.Parse(genreId));
        await _unitOfWork.Genres.DeleteAsync(genreToRemove.Id);
        await _unitOfWork.SaveAsync();
    }
}