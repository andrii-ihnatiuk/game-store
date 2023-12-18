﻿using System.Diagnostics;
using System.Text.Json;
using FluentValidation;
using GameStore.Application.Models;
using GameStore.Shared.Exceptions;
using ILogger = GameStore.Shared.Loggers.ILogger;

namespace GameStore.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

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
        ErrorDetails errorDetails = new()
        {
            Message = "An error occured during request processing.",
            Errors = new List<string> { exception.Message },
            Status = StatusCodes.Status500InternalServerError,
        };
        object response = errorDetails;

        switch (exception)
        {
            case EntityNotFoundException:
                errorDetails.Status = StatusCodes.Status404NotFound;
                break;
            case EntityAlreadyExistsException:
                errorDetails.Status = StatusCodes.Status409Conflict;
                break;
            case ForeignKeyException:
                errorDetails.Status = StatusCodes.Status400BadRequest;
                break;
            case ValidationException ex:
                errorDetails.Status = StatusCodes.Status400BadRequest;
                response = string.Join("\n", ex.Errors.Select(s => s.ErrorMessage));
                break;
            case PaymentException ex:
                response = ex.Message;
                break;
            case OrderFromNorthwindException ex:
                errorDetails.Status = StatusCodes.Status400BadRequest;
                response = ex.Message;
                break;
            case UserBannedException ex:
                response = ex.Message;
                break;
            default:
                errorDetails.Message = "Internal server error, please retry later.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorDetails.Status;
        var serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, serializerOptions));
    }
}