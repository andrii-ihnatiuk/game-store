using System.Text.Json;
using GameStore.API.Middlewares;
using GameStore.API.Models;
using GameStore.Data.Exceptions;
using Microsoft.AspNetCore.Http;

namespace GameStore.Tests.GameStore.API.Tests.Middlewares;

public class ExceptionHandlingMiddlewareTests
{
    private readonly DefaultHttpContext _context = new()
    {
        Response =
        {
            Body = new MemoryStream(),
        },
    };

    [Fact]
    public async Task Invoke_Captured_EntityNotFoundException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new EntityNotFoundException("Not found"));

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody);

        Assert.Equal(StatusCodes.Status404NotFound, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status404NotFound, errorDetails.StatusCode);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_EntityAlreadyExistsException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new EntityAlreadyExistsException());

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody);

        Assert.Equal(StatusCodes.Status409Conflict, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status409Conflict, errorDetails.StatusCode);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_ForeignKeyException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new ForeignKeyException(string.Empty));

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody);

        Assert.Equal(StatusCodes.Status400BadRequest, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, errorDetails.StatusCode);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_Exception_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new Exception());

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody);

        Assert.Equal(StatusCodes.Status500InternalServerError, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status500InternalServerError, errorDetails.StatusCode);
        Assert.Equal("Internal server error, please retry later.", errorDetails.Message);
        Assert.True(_context.Response.ContentType == "application/json");
    }
}