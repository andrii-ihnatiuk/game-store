using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
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

    public async Task<GenreFullDto> GetGenreByIdAsync(long id)
    {
        var genre = await _unitOfWork.Genres.GetOneAsync(
            g => g.Id == id,
            g => g
                .Include(nav => nav.SubGenres)
                .Include(nav => nav.Games));
        return _mapper.Map<GenreFullDto>(genre);
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync()
    {
        var genres = await _unitOfWork.Genres.GetAsync(orderBy: q => q.OrderBy(g => g.Id));
        return _mapper.Map<IList<GenreBriefDto>>(genres);
    }

    public async Task<GenreFullDto> AddGenreAsync(GenreCreateDto dto)
    {
        var genre = _mapper.Map<Genre>(dto);

        var genreExists = await _unitOfWork.Genres.GetQueryable().AnyAsync(g => g.Name == genre.Name);
        if (genreExists)
        {
            throw new EntityAlreadyExistsException(nameof(genre.Name), genre.Name);
        }

        await ThrowIfForeignKeyConstraintViolationFor(genre);
        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<GenreFullDto>(genre);
    }

    public async Task UpdateGenreAsync(GenreUpdateDto dto)
    {
        var existingGenre = await _unitOfWork.Genres.GetByIdAsync(dto.GenreId);
        _mapper.Map(dto, existingGenre);
        await ThrowIfForeignKeyConstraintViolationFor(existingGenre);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteGenreAsync(long genreId)
    {
        await _unitOfWork.Genres.DeleteAsync(genreId);
        await _unitOfWork.SaveAsync();
    }

    private async Task ThrowIfForeignKeyConstraintViolationFor(Genre genre)
    {
        bool parentExists = await _unitOfWork.Genres.GetQueryable().AnyAsync(g => g.Id == genre.ParentGenreId);

        if (!parentExists)
        {
            throw new ForeignKeyException(onColumn: nameof(genre.ParentGenreId));
        }
    }
}