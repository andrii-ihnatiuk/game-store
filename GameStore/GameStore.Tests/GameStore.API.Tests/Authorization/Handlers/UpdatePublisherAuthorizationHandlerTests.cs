using System.Linq.Expressions;
using System.Security.Claims;
using GameStore.API.Authorization.Handlers;
using GameStore.API.Authorization.Requirements;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Publisher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Authorization.Handlers;

public class UpdatePublisherAuthorizationHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly UpdatePublisherAuthorizationHandler _handler;
    private readonly CanUpdatePublisherRequirement _requirement;
    private AuthorizationHandlerContext _authContext;

    public UpdatePublisherAuthorizationHandlerTests()
    {
        _handler = new UpdatePublisherAuthorizationHandler(_unitOfWorkMock.Object);
        _requirement = new CanUpdatePublisherRequirement();
    }

    [Theory]
    [InlineData(PermissionOptions.PublisherFull)]
    [InlineData(PermissionOptions.PublisherUpdate)]
    public async Task HandleRequirementAsync_HaveUpdatePermissions_SucceedResult(string permission)
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(CustomClaimTypes.Permission, permission) }));
        SetupAuthContext(principal);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_HasUserIdButNoPermissionToUpdateSelf_FailedResult()
    {
        // Arrange
        const string id = "test";
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, id) }));
        SetupAuthContext(principal);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasFailed);
    }

    [Fact]
    public async Task HandleRequirementAsync_NoUserId_FailedResult()
    {
        // Arrange
        SetupAuthContext(new ClaimsPrincipal());

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasFailed);
    }

    [Fact]
    public async Task HandleRequirementAsync_CanUpdateSelfButPublisherFromNorthwind_FailedResult()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "id"),
            new Claim(CustomClaimTypes.Permission, PermissionOptions.PublisherUpdateSelf),
        }));
        var dto = new PublisherUpdateDto { Publisher = new PublisherUpdateInnerDto { Id = "not-guid" } };
        SetupAuthContext(principal, dto);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasFailed);
    }

    [Fact]
    public async Task HandleRequirementAsync_CanUpdateSelfAndIdMatch_SucceedResult()
    {
        // Arrange
        const string id = "id";
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(CustomClaimTypes.Permission, PermissionOptions.PublisherUpdateSelf),
        }));
        var dto = new PublisherUpdateDto { Publisher = new PublisherUpdateInnerDto { Id = Guid.Empty.ToString() } };
        SetupAuthContext(principal, dto);

        _unitOfWorkMock.Setup(s => s.Publishers.GetOneAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new Publisher { AccountId = id })
            .Verifiable(Times.Once);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasSucceeded);
        _unitOfWorkMock.Verify();
    }

    [Fact]
    public async Task HandleRequirementAsync_CanUpdateSelfButIdMismatch_FailedResult()
    {
        // Arrange
        const string id = "id";
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(CustomClaimTypes.Permission, PermissionOptions.PublisherUpdateSelf),
        }));
        var dto = new PublisherUpdateDto { Publisher = new PublisherUpdateInnerDto { Id = Guid.Empty.ToString() } };
        SetupAuthContext(principal, dto);

        _unitOfWorkMock.Setup(s => s.Publishers.GetOneAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new Publisher { AccountId = "other id" })
            .Verifiable(Times.Once);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasFailed);
        _unitOfWorkMock.Verify();
    }

    private void SetupAuthContext(ClaimsPrincipal principal, PublisherUpdateDto? resource = null)
    {
        IEnumerable<IAuthorizationRequirement> requirements = new List<IAuthorizationRequirement> { _requirement };
        _authContext = new AuthorizationHandlerContext(requirements, principal, resource ?? new PublisherUpdateDto());
    }
}