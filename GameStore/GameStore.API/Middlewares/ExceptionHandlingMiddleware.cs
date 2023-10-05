using System.Text.Json;
using GameStore.API.Models;
using GameStore.Shared.Exceptions;

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
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        ErrorDetails errorDetails = default;

        switch (exception)
        {
            case EntityNotFoundException ex:
                errorDetails.StatusCode = StatusCodes.Status404NotFound;
                errorDetails.Message = ex.Message;
                break;
            default:
                errorDetails.StatusCode = StatusCodes.Status500InternalServerError;
                errorDetails.Message = "Internal server error, please retry later.";
                break;
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }
}