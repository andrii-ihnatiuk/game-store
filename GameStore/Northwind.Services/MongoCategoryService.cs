using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Interfaces.Services;
using Northwind.Data.Interfaces;

namespace Northwind.Services;

public class MongoCategoryService : MongoServiceBase, IGenreService
{
    private readonly IMongoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MongoCategoryService(IMongoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GenreFullDto> GetGenreByIdAsync(string id)
    {
        var category = await _unitOfWork.Categories.GetOneAsync(c => c.Id == id);
        return _mapper.Map<GenreFullDto>(category);
    }

    public async Task<IList<GenreBriefDto>> GetAllGenresAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return _mapper.Map<IList<GenreBriefDto>>(categories);
    }

    public async Task<IList<GenreBriefDto>> GetSubgenresByParentAsync(string parentId)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(c => c.ParentId == parentId);
        return _mapper.Map<IList<GenreBriefDto>>(categories);
    }

    public async Task<IList<GameBriefDto>> GetGamesByGenreId(string id)
    {
        var games = await _unitOfWork.Categories.GetProductsByCategoryIdAsync(id);
        return _mapper.Map<IList<GameBriefDto>>(games);
    }

    public Task<GenreBriefDto> AddGenreAsync(GenreCreateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateGenreAsync(GenreUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGenreAsync(string genreId)
    {
        throw new NotImplementedException();
    }
}