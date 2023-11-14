using AutoMapper;
using GameStore.Shared.DTOs.Order;
using Northwind.Data.Interfaces;
using Northwind.Services.Interfaces;

namespace Northwind.Services;

public class OrderService : IOrderService
{
    private readonly IMongoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IMongoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<OrderBriefDto>> GetPaidOrdersByCustomerAsync(string customerId)
    {
        var orders = await _unitOfWork.Orders.GetAllAsync(o => o.CustomerId == customerId);
        return _mapper.Map<IList<OrderBriefDto>>(orders);
    }
}