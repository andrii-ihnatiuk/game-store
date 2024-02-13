using GameStore.Services.Interfaces.Payment;
using GameStore.Services.Payment;
using GameStore.Shared.Constants;
using GameStore.Shared.Exceptions;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class PaymentStrategyResolverTests
{
    private readonly Mock<IPaymentStrategy> _bankStrategy;
    private readonly Mock<IPaymentStrategy> _terminalStrategy;
    private readonly PaymentStrategyResolver _resolver;

    public PaymentStrategyResolverTests()
    {
        _bankStrategy = new Mock<IPaymentStrategy>();
        _terminalStrategy = new Mock<IPaymentStrategy>();
        List<IPaymentStrategy> strategies = new() { _bankStrategy.Object, _terminalStrategy.Object };
        _resolver = new PaymentStrategyResolver(strategies);
    }

    [Fact]
    public void Resolve_FindsCorrectStrategy()
    {
        // Arrange
        _bankStrategy.Setup(s => s.Name).Returns(PaymentStrategyName.Bank);
        _terminalStrategy.Setup(s => s.Name).Returns(PaymentStrategyName.Terminal);

        // Act
        var strategy = _resolver.Resolve(PaymentStrategyName.Terminal);

        // Assert
        Assert.Equal(_terminalStrategy.Object, strategy);
    }

    [Fact]
    public void Resolve_ThrowsExceptionWhenStrategyNotFound()
    {
        // Arrange
        _bankStrategy.Setup(s => s.Name).Returns(PaymentStrategyName.Bank);
        _terminalStrategy.Setup(s => s.Name).Returns(PaymentStrategyName.Terminal);

        // Assert
        Assert.Throws<PaymentException>(() => _resolver.Resolve(PaymentStrategyName.Visa));
    }
}