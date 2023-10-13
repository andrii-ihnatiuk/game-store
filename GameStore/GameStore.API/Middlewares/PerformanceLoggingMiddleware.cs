using System.Diagnostics;
using ILogger = GameStore.Shared.Loggers.ILogger;

namespace GameStore.API.Middlewares;

public class PerformanceLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public PerformanceLoggingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = new Stopwatch();
        sw.Start();

        await _next(context);

        sw.Stop();
        _logger.LogInfo($"Processing request [{context.Request.Method}] {context.Request.Path} took {sw.ElapsedMilliseconds} ms");
    }
}