using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CoreGenreService : CoreServiceBase, ICoreGenreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CoreGenreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreFullDto> GetGenreByIdAsync(string id)
    {
        var genreId = Guid.Parse(id);
        var genre = await _unitOfWork.Genres.GetOneAsync(
            g => g.Id == genreId,
            g => g
                .Include(nav => nav.SubGenres));
        return _mapper.Map<GenreFullDto>(genre);
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync()
    {
        var genres = await _unitOfWork.Genres.GetAsync(orderBy: q => q.OrderBy(g => g.Id));
        return _mapper.Map<IList<GenreBriefDto>>(genres);
    }

    public async Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId)
    {
        var genreId = Guid.Parse(parentId);
        var subgenres = await _unitOfWork.Genres.GetAsync(predicate: g => g.ParentGenreId == genreId);
        return _mapper.Map<IList<GenreBriefDto>>(subgenres);
    }

    public async Task<IList<GameBriefDto>> GetGamesByGenreIdAsync(string id)
    {
        var genreId = Guid.Parse(id);
        var games = (await _unitOfWork.GamesGenres.GetAsync(
                predicate: gg => gg.GenreId == genreId,
                include: q => q.Include(gg => gg.Game)))
            .Select(gg => gg.Game);
        return _mapper.Map<IList<GameBriefDto>>(games);
    }

    public async Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto)
    {
        var genre = _mapper.Map<Genre>(dto);
        await _unitOfWork.Genres.ThrowIfGenreNameIsNotUnique(genre.Name);
        await _unitOfWork.Genres.ThrowIfForeignKeyConstraintViolation(genre);
        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<GenreBriefDto>(genre);
    }

    public async Task UpdateGenreAsync(GenreUpdateDto dto)
    {
        var existingGenre = await _unitOfWork.Genres.GetByIdAsync(Guid.Parse(dto.Genre.Id));
        if (existingGenre.Name != dto.Genre.Name)
        {
            await _unitOfWork.Genres.ThrowIfGenreNameIsNotUnique(dto.Genre.Name);
        }

        var updatedGenre = _mapper.Map(dto, existingGenre);

        await _unitOfWork.Genres.ThrowIfForeignKeyConstraintViolation(updatedGenre);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGenreAsync(string genreId)
    {
        var genreToRemove = await _unitOfWork.Genres.GetOneAsync(g => g.Id == Guid.Parse(genreId));
        await _unitOfWork.Genres.DeleteAsync(genreToRemove.Id);
        await _unitOfWork.SaveAsync();
    }
}