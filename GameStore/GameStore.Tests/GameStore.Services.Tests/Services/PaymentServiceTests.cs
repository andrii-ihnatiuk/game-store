using System.Linq.Expressions;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Payment;
using GameStore.Shared.DTOs.Order;
using GameStore.Shared.DTOs.Payment;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentStrategyResolver> _strategyResolver = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        _service = new PaymentService(_strategyResolver.Object, _unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetAvailablePaymentMethodsAsync_ReturnsCorrectAmountOfPaymentMethods()
    {
        // Arrange
        var methods = new List<PaymentMethod> { new() };
        _unitOfWork.Setup(u => u.PaymentMethods.GetAsync(
                It.IsAny<Expression<Func<PaymentMethod, bool>>>(),
                It.IsAny<Func<IQueryable<PaymentMethod>, IOrderedQueryable<PaymentMethod>>>(),
                It.IsAny<Func<IQueryable<PaymentMethod>, IIncludableQueryable<PaymentMethod, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(methods);

        var methodDtos = new List<PaymentMethodDto> { new() };
        _mapper.Setup(m => m.Map<IList<PaymentMethodDto>>(It.IsAny<IEnumerable<PaymentMethod>>()))
            .Returns(methodDtos);

        // Act
        var result = await _service.GetAvailablePaymentMethodsAsync();

        // Assert
        Assert.Equal(methodDtos.Count, result.Count);
    }

    [Fact]
    public void RequestPaymentAsync_CallsCorrectStrategy()
    {
        // Arrange
        var payment = new PaymentDto { Method = "Bank" };
        var strategy = new Mock<IPaymentStrategy>();
        _strategyResolver.Setup(s => s.Resolve(payment.Method)).Returns(strategy.Object);
        var paymentResult = new Mock<IPaymentResult>();
        strategy.Setup(s => s.ProcessPayment(payment, It.IsAny<Guid>())).ReturnsAsync(paymentResult.Object);

        // Act
        var result = _service.RequestPaymentAsync(payment, Guid.NewGuid());

        // Assert
        strategy.Verify(s => s.ProcessPayment(payment, It.IsAny<Guid>()), Times.Once);
    }
}