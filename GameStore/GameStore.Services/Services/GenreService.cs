using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.Services;

public class GenreService : IGenreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreFullDto> GetGenreByIdAsync(Guid id)
    {
        var genre = await _unitOfWork.Genres.GetOneAsync(
            g => g.Id == id,
            g => g
                .Include(nav => nav.SubGenres));
        return _mapper.Map<GenreFullDto>(genre);
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync()
    {
        var genres = await _unitOfWork.Genres.GetAsync(orderBy: q => q.OrderBy(g => g.Id));
        return _mapper.Map<IList<GenreBriefDto>>(genres);
    }

    public async Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(Guid parentId)
    {
        var subgenres = await _unitOfWork.Genres.GetAsync(predicate: g => g.ParentGenreId == parentId);
        return _mapper.Map<IList<GenreBriefDto>>(subgenres);
    }

    public async Task<IList<GameBriefDto>> GetGamesByGenreId(Guid id)
    {
        var games = (await _unitOfWork.GamesGenres.GetAsync(
                predicate: gg => gg.GenreId == id,
                include: q => q.Include(gg => gg.Game)))
            .Select(gg => gg.Game);
        return _mapper.Map<IList<GameBriefDto>>(games);
    }

    public async Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto)
    {
        var genre = _mapper.Map<Genre>(dto);
        await ThrowIfGenreNameIsNotUnique(genre.Name);
        await ThrowIfForeignKeyConstraintViolationFor(genre);
        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<GenreBriefDto>(genre);
    }

    public async Task UpdateGenreAsync(GenreUpdateDto dto)
    {
        var existingGenre = await _unitOfWork.Genres.GetByIdAsync(dto.Genre.Id);
        if (existingGenre.Name != dto.Genre.Name)
        {
            await ThrowIfGenreNameIsNotUnique(dto.Genre.Name);
        }

        var updatedGenre = _mapper.Map(dto, existingGenre);
        await ThrowIfForeignKeyConstraintViolationFor(updatedGenre);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGenreAsync(Guid genreId)
    {
        await _unitOfWork.Genres.DeleteAsync(genreId);
        await _unitOfWork.SaveAsync();
    }

    private async Task ThrowIfForeignKeyConstraintViolationFor(Genre genre)
    {
        if (genre.ParentGenreId != null)
        {
            bool parentExists = await _unitOfWork.Genres.ExistsAsync(g => g.Id == genre.ParentGenreId);
            if (!parentExists || genre.Id == genre.ParentGenreId)
            {
                throw new ForeignKeyException(onColumn: nameof(genre.ParentGenreId));
            }
        }
    }

    private async Task ThrowIfGenreNameIsNotUnique(string name)
    {
        bool nameIsNotUnique = await _unitOfWork.Genres.ExistsAsync(g => g.Name == name);
        if (nameIsNotUnique)
        {
            throw new EntityAlreadyExistsException(nameof(Genre.Name), name);
        }
    }
}