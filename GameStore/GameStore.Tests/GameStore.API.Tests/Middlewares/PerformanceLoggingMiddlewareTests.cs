using GameStore.API.Middlewares;
using GameStore.Shared.Loggers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Middlewares;

public class PerformanceLoggingMiddlewareTests
{
    private readonly Mock<ILogger> _logger = new();

    [Fact]
    public async Task Middleware_LogRequestProcessingDuration()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Request =
            {
                Method = HttpMethods.Get,
                Path = "/test",
            },
        };
        var middleware = new PerformanceLoggingMiddleware(_ => Task.CompletedTask, _logger.Object);

        // Act
        await middleware.Invoke(context);

        // Assert
        _logger.Verify(l => l.LogInfo(It.Is<string>(s => s.StartsWith("Processing request [GET] /test took"))), Times.Once());
    }
}