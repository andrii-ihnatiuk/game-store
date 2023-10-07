using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Exceptions;
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

    public async Task<GenreViewFullDto> GetGenreByIdAsync(long id)
    {
        var genre = await _unitOfWork.Genres.FirstOrDefaultAsync(
            g => g.Id == id,
            g => g.Include(nav => nav.SubGenres));
        return genre is null ? throw new EntityNotFoundException(entityId: id) : _mapper.Map<GenreViewFullDto>(genre);
    }

    public async Task<GenreViewFullDto> AddGenreAsync(GenreCreateDto dto)
    {
        var genre = _mapper.Map<Genre>(dto);

        var genreExists = await _unitOfWork.Genres.GetQueryable().AnyAsync(g => g.Name == genre.Name);
        if (genreExists)
        {
            throw new EntityAlreadyExistsException(nameof(genre.Name), genre.Name);
        }

        await _unitOfWork.Genres.AddAsync(genre);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<GenreViewFullDto>(genre);
    }
}