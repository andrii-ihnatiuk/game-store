using AutoMapper;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.Interfaces.Services;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace Northwind.Services;

public class MongoOrderService : IOrderService
{
    private readonly IMongoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MongoOrderService(IMongoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<OrderBriefDto>> GetPaidOrdersByCustomerAsync(string customerId, DateTime lowerDate, DateTime upperDate)
    {
        var orders = await _unitOfWork.Orders.GetAllAsync(
            o => o.CustomerId == customerId
                 && o.OrderDate >= lowerDate
                 && o.OrderDate <= upperDate);
        return _mapper.Map<IList<OrderBriefDto>>(orders);
    }

    public async Task<OrderBriefDto> GetOrderByIdAsync(string orderId)
    {
        var order = await _unitOfWork.Orders.GetOneAsync(o => o.Id == orderId);
        return _mapper.Map<OrderBriefDto>(order);
    }

    public Task<IList<OrderDetailDto>> GetOrderDetailsAsync(string orderId)
    {
        // var order = await _unitOfWork.Orders.GetOneAsync(o => o.Id == orderId);
        // var details = await _unitOfWork.OrderDetails.GetAllAsync(od => od.OrderId == order.OrderId);
        var details = new List<OrderDetail>();
        return Task.FromResult(_mapper.Map<IList<OrderDetailDto>>(details));
    }
}