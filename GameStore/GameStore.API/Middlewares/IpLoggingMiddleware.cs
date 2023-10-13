using ILogger = GameStore.Shared.Loggers.ILogger;

namespace GameStore.API.Middlewares;

public class IpLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public IpLoggingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        _logger.LogInfo($"IP: {ipAddress}");
        await _next(context);
    }
}