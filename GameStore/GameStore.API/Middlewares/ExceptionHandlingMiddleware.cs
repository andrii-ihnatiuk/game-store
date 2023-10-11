using System.Text.Json;
using GameStore.API.Models;
using GameStore.Data.Exceptions;

namespace GameStore.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
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
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ErrorDetails errorDetails = new()
        {
            Message = exception.Message,
        };

        switch (exception)
        {
            case EntityNotFoundException:
                errorDetails.StatusCode = StatusCodes.Status404NotFound;
                break;
            case EntityAlreadyExistsException:
                errorDetails.StatusCode = StatusCodes.Status409Conflict;
                break;
            case ForeignKeyException:
                errorDetails.StatusCode = StatusCodes.Status400BadRequest;
                break;
            default:
                errorDetails.StatusCode = StatusCodes.Status500InternalServerError;
                errorDetails.Message = "Internal server error, please retry later.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorDetails.StatusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }
}