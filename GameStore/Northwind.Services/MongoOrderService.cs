using AutoMapper;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Interfaces.Services;
using GameStore.Shared.Models;
using Northwind.Data.Interfaces;

namespace Northwind.Services;

public class MongoOrderService : MongoServiceBase, IOrderService
{
    private readonly IMongoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MongoOrderService(IMongoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<OrderBriefDto>> GetFilteredOrdersAsync(OrdersFilter filter)
    {
        var orders = await _unitOfWork.Orders.GetFilteredOrdersAsync(filter);
        return _mapper.Map<IList<OrderBriefDto>>(orders);
    }

    public async Task<OrderBriefDto> GetOrderByIdAsync(string orderId)
    {
        var order = await _unitOfWork.Orders.GetOneAsync(o => o.Id == orderId);
        return _mapper.Map<OrderBriefDto>(order);
    }

    public async Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId)
    {
        var details = await _unitOfWork.OrderDetails.GetAllByOrderObjectIdAsync(orderId);
        return _mapper.Map<IList<OrderDetailDto>>(details);
    }
}