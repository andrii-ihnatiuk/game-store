using GameStore.API.Controllers;
using GameStore.Services.Interfaces;
using GameStore.Services.Models;
using GameStore.Shared.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class OrdersControllerTests
{
    private const string GameAlias = "testGame";
    private static readonly Guid OrderId = Guid.NewGuid();
    private readonly Mock<IOrderService> _orderService = new();
    private readonly Mock<IPaymentService> _paymentService = new();
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _controller = new OrdersController(_orderService.Object, _paymentService.Object);
    }

    [Fact]
    public async Task AddGameToCartAsync_ReturnsOkResult()
    {
        // Arrange
        _orderService.Setup(o => o.AddGameToCartAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddGameToCartAsync(GameAlias);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetCartByCustomerAsync_ReturnsOkResultWithOrderDetailList()
    {
        // Arrange
        var orderDetails = new List<OrderDetailDto> { new(), new() };
        _orderService.Setup(o => o.GetCartByCustomerAsync(It.IsAny<Guid>())).ReturnsAsync(orderDetails);

        // Act
        var result = await _controller.GetCartByCustomerAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<OrderDetailDto>>(okResult.Value);
        Assert.Equal(orderDetails, returnValue);
    }

    [Fact]
    public async Task GetPaidOrdersByCustomerAsync_ReturnsOkResultWithListOfOrderBriefDto()
    {
        // Arrange
        var paidOrders = new List<OrderBriefDto> { new(), new() };
        _orderService.Setup(o => o.GetPaidOrdersByCustomerAsync(It.IsAny<Guid>())).ReturnsAsync(paidOrders);

        // Act
        var result = await _controller.GetPaidOrdersByCustomerAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<OrderBriefDto>>(okResult.Value);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ReturnsOkResultWithOrderBriefDto()
    {
        // Arrange
        var order = new OrderBriefDto();
        _orderService.Setup(o => o.GetOrderByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order);

        // Act
        var result = await _controller.GetOrderByIdAsync(OrderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<OrderBriefDto>(okResult.Value);
        Assert.Equal(order, returnValue);
    }

    [Fact]
    public async Task GetOrderDetailsAsync_ReturnsOkResultWithOrderDetailDtoList()
    {
        // Arrange
        var orderDetails = new List<OrderDetailDto> { new(), new() };
        _orderService.Setup(o => o.GetOrderDetailsAsync(It.IsAny<Guid>())).ReturnsAsync(orderDetails);

        // Act
        var result = await _controller.GetOrderDetailsAsync(OrderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<OrderDetailDto>>(okResult.Value);
        Assert.Equal(orderDetails.Count, returnValue.Count());
    }

    [Fact]
    public async Task GetAvailablePaymentMethodsAsync_ReturnsOkResultWithPaymentMethodListDto()
    {
        // Arrange
        var paymentMethodsDto = new List<PaymentMethodDto> { new(), new() };
        _paymentService.Setup(o => o.GetAvailablePaymentMethodsAsync()).ReturnsAsync(paymentMethodsDto);

        // Act
        var result = await _controller.GetAvailablePaymentMethodsAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<PaymentMethodListDto>(okResult.Value);
        Assert.Equal(paymentMethodsDto.Count, returnValue.PaymentMethods.Count);
    }

    [Fact]
    public async Task PayForOrderAsync_ReturnsBadResultWhenPaymentResultIsNull()
    {
        // Arrange
        var paymentDto = new PaymentDto();
        _paymentService.Setup(o => o.RequestPaymentAsync(It.IsAny<PaymentDto>(), It.IsAny<Guid>()))
            .ReturnsAsync(default(BankPaymentResult));

        // Act
        var result = await _controller.PayForOrderAsync(paymentDto);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task PayForOrderAsync_WhenBankPayment_ReturnsFileContentResult()
    {
        // Arrange
        var paymentDto = new PaymentDto();
        var paymentResult = new BankPaymentResult { InvoiceFileBytes = Array.Empty<byte>(), ContentType = "application/pdf", FileDownloadName = "invoice.pdf" };
        _paymentService.Setup(o => o.RequestPaymentAsync(It.IsAny<PaymentDto>(), It.IsAny<Guid>())).ReturnsAsync(paymentResult);

        // Act
        var result = await _controller.PayForOrderAsync(paymentDto);

        // Assert
        var fileContentResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal(paymentResult.InvoiceFileBytes, fileContentResult.FileContents);
        Assert.Equal(paymentResult.ContentType, fileContentResult.ContentType);
        Assert.Equal(paymentResult.FileDownloadName, fileContentResult.FileDownloadName);
    }

    [Fact]
    public async Task DeleteGameFromCartAsync_ReturnsNoContentResult()
    {
        // Arrange
        _orderService.Setup(o => o.DeleteGameFromCartAsync(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGameFromCartAsync(GameAlias);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}