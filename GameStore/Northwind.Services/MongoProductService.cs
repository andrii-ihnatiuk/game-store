using System.Text;
using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using GameStore.Shared.Models;
using GameStore.Shared.Util;
using Northwind.Data.Interfaces;

namespace Northwind.Services;

public class MongoProductService : MongoServiceBase, IGameService
{
    private readonly IMongoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MongoProductService(IMongoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GameFullDto> GetGameByAliasAsync(string alias)
    {
        alias = EntityAliasUtil.RemoveSuffix(alias);
        var product = await _unitOfWork.Products.GetOneAsync(p => p.Alias == alias);
        return _mapper.Map<GameFullDto>(product);
    }

    public async Task<GameFullDto> GetGameByIdAsync(string id)
    {
        long productId = long.Parse(id);
        var product = await _unitOfWork.Products.GetOneAsync(p => p.ProductId == productId);
        return _mapper.Map<GameFullDto>(product);
    }

    public async Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias)
    {
        alias = EntityAliasUtil.RemoveSuffix(alias);
        var categories = await _unitOfWork.Products.GetCategoriesByProductAliasAsync(alias);
        return _mapper.Map<IList<GenreBriefDto>>(categories);
    }

    public Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias)
    {
        return Task.FromResult((IList<PlatformBriefDto>)new List<PlatformBriefDto>());
    }

    public async Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias)
    {
        alias = EntityAliasUtil.RemoveSuffix(alias);
        var categories = await _unitOfWork.Products.GetSupplierByProductAliasAsync(alias);
        return _mapper.Map<PublisherBriefDto>(categories);
    }

    public async Task<EntityFilteringResult<GameFullDto>> GetAllGamesAsync(GamesFilter filter)
    {
        IList<GameFullDto> records = new List<GameFullDto>();
        var totalNoLimit = 0;

        if (filter.Platforms.Count == 0 && filter.DatePublishing is null)
        {
            var filteringResult = await _unitOfWork.Products.GetFilteredProductsAsync(filter);
            records = _mapper.Map<IList<GameFullDto>>(filteringResult.Records);
            totalNoLimit = filteringResult.TotalNoLimit;
        }

        return new EntityFilteringResult<GameFullDto>(records, totalNoLimit);
    }

    public async Task<Tuple<byte[], string>> DownloadAsync(string gameAlias)
    {
        var game = await _unitOfWork.Products.GetOneAsync(g => g.Alias == EntityAliasUtil.RemoveSuffix(gameAlias));
        string content = $"Game: {game.ProductName}\n\nDescription:";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string fileName = $"{game.ProductName}_{timestamp}.txt";
        return new Tuple<byte[], string>(bytes, fileName);
    }
}