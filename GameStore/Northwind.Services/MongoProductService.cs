using AutoMapper;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using Northwind.Data.Interfaces;

namespace Northwind.Services;

public class MongoProductService : IGameService
{
    private readonly IMongoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MongoProductService(IMongoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public EntityStorage EntityStorage => EntityStorage.MongoDb;

    public Task<GameFullDto> GetGameByAliasAsync(string alias)
    {
        throw new NotImplementedException();
    }

    public async Task<GameFullDto> GetGameByIdAsync(string id)
    {
        long productId = long.Parse(id);
        var product = await _unitOfWork.Products.GetOneAsync(p => p.ProductId == productId);
        return _mapper.Map<GameFullDto>(product);
    }

    public Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias)
    {
        throw new NotImplementedException();
    }

    public Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias)
    {
        throw new NotImplementedException();
    }

    public Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias)
    {
        throw new NotImplementedException();
    }

    public Task<FilteredGamesDto> GetAllGamesAsync(GamesFilterDto filterDto)
    {
        throw new NotImplementedException();
    }

    public Task<Tuple<byte[], string>> DownloadAsync(string gameAlias)
    {
        throw new NotImplementedException();
    }
}