using System.Text.Json;
using GameStore.API.Middlewares;
using GameStore.Data.Exceptions;
using GameStore.Services.Models;
using GameStore.Shared.Loggers;
using Microsoft.AspNetCore.Http;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Middlewares;

public class ExceptionHandlingMiddlewareTests
{
    private readonly Mock<ILogger> _logger = new();
    private readonly DefaultHttpContext _context = new()
    {
        Response =
        {
            Body = new MemoryStream(),
        },
    };

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact]
    public async Task Invoke_Captured_EntityNotFoundException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new EntityNotFoundException("Not found"), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody, _serializerOptions);

        Assert.Equal(StatusCodes.Status404NotFound, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status404NotFound, errorDetails.Status);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_EntityAlreadyExistsException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new EntityAlreadyExistsException(), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody, _serializerOptions);

        Assert.Equal(StatusCodes.Status409Conflict, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status409Conflict, errorDetails.Status);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_ForeignKeyException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new ForeignKeyException(string.Empty), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody, _serializerOptions);

        Assert.Equal(StatusCodes.Status400BadRequest, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, errorDetails.Status);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_Exception_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new Exception(), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        var errorDetails = JsonSerializer.Deserialize<ErrorDetails>(responseBody, _serializerOptions);

        Assert.Equal(StatusCodes.Status500InternalServerError, _context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status500InternalServerError, errorDetails.Status);
        Assert.Equal("Internal server error, please retry later.", errorDetails.Message);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_Exception_LoggedError()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new Exception(), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _logger.Verify(l => l.LogError(It.IsAny<string>()), Times.Once);
    }
}