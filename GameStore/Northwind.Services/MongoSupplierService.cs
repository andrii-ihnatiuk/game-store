using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Interfaces.Services;
using Northwind.Data.Interfaces;

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

    public async Task<PublisherFullDto> GetPublisherByIdAsync(string id, string culture)
    {
        var publisher = await _unitOfWork.Suppliers.GetOneAsync(s => s.Id == id);
        var dto = _mapper.Map<PublisherFullDto>(publisher);
        return dto;
    }

    public async Task<IList<PublisherBriefDto>> GetAllPublishersAsync(string culture)
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
        return _mapper.Map<IList<PublisherBriefDto>>(suppliers);
    }

    public async Task<IList<GameBriefDto>> GetGamesByPublisherIdAsync(string id, string culture)
    {
        var products = await _unitOfWork.Suppliers.GetProductsBySupplierIdAsync(id);
        return _mapper.Map<IList<GameBriefDto>>(products);
    }

    public Task DeletePublisherAsync(string id)
    {
        _unitOfWork.Suppliers.DeleteAsync(id);
        return _unitOfWork.SaveChangesAsync();
    }
}