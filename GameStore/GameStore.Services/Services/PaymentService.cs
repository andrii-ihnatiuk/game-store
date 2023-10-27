using AutoMapper;
using GameStore.Data.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Order;

namespace GameStore.Services.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentStrategyResolver _strategyResolver;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaymentService(IPaymentStrategyResolver strategyResolver, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _strategyResolver = strategyResolver;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<PaymentMethodDto>> GetAvailablePaymentMethodsAsync()
    {
        var methods = await _unitOfWork.PaymentMethods.GetAsync();
        return _mapper.Map<IList<PaymentMethodDto>>(methods);
    }

    public Task<IPaymentResult> RequestPaymentAsync(PaymentDto payment, Guid customerId)
    {
        var strategy = _strategyResolver.Resolve(payment.Method);
        return strategy.ProcessPayment(payment, customerId);
    }
}