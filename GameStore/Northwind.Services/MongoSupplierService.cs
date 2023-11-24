using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using Northwind.Data.Interfaces;
using Northwind.Services.Util;

namespace Northwind.Services;

public class MongoSupplierService : MongoServiceBase, IPublisherService
{
    private readonly IMongoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MongoSupplierService(IMongoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PublisherFullDto> GetPublisherByNameAsync(string companyName)
    {
        companyName = EntityAliasUtil.RemoveSuffix(companyName);
        var publisher = await _unitOfWork.Suppliers.GetOneAsync(s => s.CompanyName == companyName);
        var dto = _mapper.Map<PublisherFullDto>(publisher);
        return dto;
    }

    public async Task<IList<PublisherBriefDto>> GetAllPublishersAsync()
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
        return _mapper.Map<IList<PublisherBriefDto>>(suppliers);
    }

    public async Task<IList<GameBriefDto>> GetGamesByPublisherNameAsync(string companyName)
    {
        companyName = EntityAliasUtil.RemoveSuffix(companyName);
        var products = await _unitOfWork.Suppliers.GetProductsBySupplierNameAsync(companyName);
        return _mapper.Map<IList<GameBriefDto>>(products);
    }

    public Task UpdatePublisherAsync(PublisherUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeletePublisherAsync(string id)
    {
        throw new NotImplementedException();
    }
}