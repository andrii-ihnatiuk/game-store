using System.Diagnostics;
using System.Net.Mime;
using FluentValidation;
using GameStore.Shared.Exceptions;
using ILogger = GameStore.Shared.Loggers.ILogger;

namespace GameStore.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    private static readonly Type[] VisibleMessageExceptions =
    {
        typeof(EntityNotFoundException), typeof(EntityAlreadyExistsException), typeof(ForeignKeyException),
        typeof(PaymentException), typeof(GameStoreNotSupportedException), typeof(UserBannedException),
        typeof(IdentityException), typeof(LoginException),
    };

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
            StackTrace trace = new(ex, true);
            string? method = trace.GetFrame(0)?.GetMethod()?.ReflectedType?.FullName;
            _logger.LogError($"An error occurred in {method}: {ex.Message}");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = exception switch
        {
            ForeignKeyException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            GameStoreNotSupportedException => StatusCodes.Status400BadRequest,
            IdentityException => StatusCodes.Status400BadRequest,
            LoginException => StatusCodes.Status401Unauthorized,
            PaymentException => StatusCodes.Status402PaymentRequired,
            UserBannedException => StatusCodes.Status403Forbidden,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            EntityAlreadyExistsException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        string responseMessage = exception switch
        {
            ValidationException ex => string.Join("\n", ex.Errors.Select(s => s.ErrorMessage)),
            _ when VisibleMessageExceptions.Contains(exception.GetType()) => exception.Message,
            _ => "Internal server error, please retry later.",
        };

        context.Response.ContentType = MediaTypeNames.Text.Plain;
        await context.Response.WriteAsync(responseMessage);
    }
}