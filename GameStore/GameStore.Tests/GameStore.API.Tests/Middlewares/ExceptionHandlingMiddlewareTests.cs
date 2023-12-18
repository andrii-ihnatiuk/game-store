using FluentValidation.Results;
using GameStore.API.Middlewares;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Loggers;
using Microsoft.AspNetCore.Http;
using Moq;
using ValidationException = FluentValidation.ValidationException;

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

    [Fact]
    public async Task Invoke_Captured_EntityNotFoundException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new EntityNotFoundException("Not found"), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, _context.Response.StatusCode);
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
        Assert.Equal(StatusCodes.Status409Conflict, _context.Response.StatusCode);
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
        Assert.Equal(StatusCodes.Status400BadRequest, _context.Response.StatusCode);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_ValidationException_HandledCorrectly()
    {
        // Arrange
        IEnumerable<ValidationFailure> failures = new[] { new ValidationFailure("property", "Validation error message") };
        var validationException = new ValidationException("Validation failed", errors: failures);
        var middleware = new ExceptionHandlingMiddleware(_ => throw validationException, _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        Assert.Contains("Validation error message", responseBody);
        Assert.Equal(StatusCodes.Status400BadRequest, _context.Response.StatusCode);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_PaymentException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new PaymentException("Payment failed"), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();
        Assert.Contains("Payment failed", responseBody);
        Assert.Equal(StatusCodes.Status500InternalServerError, _context.Response.StatusCode);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_UserBannedException_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new UserBannedException("User is banned"), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        _context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(_context.Response.Body);
        string responseBody = await reader.ReadToEndAsync();

        Assert.Contains("User is banned", responseBody);
        Assert.Equal(StatusCodes.Status403Forbidden, _context.Response.StatusCode);
        Assert.True(_context.Response.ContentType == "application/json");
    }

    [Fact]
    public async Task Invoke_Captured_OrderFromNorthwind_HandledCorrectly()
    {
        // Arrange
        var middleware = new ExceptionHandlingMiddleware(_ => throw new OrderFromNorthwindException(), _logger.Object);

        // Act
        await middleware.Invoke(_context);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, _context.Response.StatusCode);
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

        Assert.Equal(StatusCodes.Status500InternalServerError, _context.Response.StatusCode);
        Assert.Equal("Internal server error, please retry later.", responseBody);
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