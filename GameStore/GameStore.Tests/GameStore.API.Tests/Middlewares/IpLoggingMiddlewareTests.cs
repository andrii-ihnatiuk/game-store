using System.Net;
using GameStore.API.Middlewares;
using GameStore.Shared.Loggers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Middlewares;

public class IpLoggingMiddlewareTests
{
    private readonly Mock<ILogger> _logger = new();

    [Fact]
    public async Task Middleware_LogsRemoteIpAddress()
    {
        // Arrange
        var context = new DefaultHttpContext
        {
            Connection =
            {
                RemoteIpAddress = IPAddress.Parse("127.0.0.1"),
            },
        };
        var middleware = new IpLoggingMiddleware(_ => Task.CompletedTask, _logger.Object);

        // Act
        await middleware.Invoke(context);

        // Assert
        _logger.Verify(l => l.LogInfo("IP: 127.0.0.1"), Times.Once);
    }
}