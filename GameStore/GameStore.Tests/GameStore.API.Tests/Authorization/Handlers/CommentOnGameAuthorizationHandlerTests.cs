using System.Linq.Expressions;
using System.Security.Claims;
using GameStore.API.Authorization.Handlers;
using GameStore.API.Authorization.Requirements;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Authorization.Handlers;

public class CommentOnGameAuthorizationHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CommentOnGameAuthorizationHandler _handler;
    private readonly CanCommentOnGameRequirement _requirement;
    private AuthorizationHandlerContext _authContext;

    public CommentOnGameAuthorizationHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CommentOnGameAuthorizationHandler(_unitOfWorkMock.Object);
        _requirement = new CanCommentOnGameRequirement();
    }

    [Theory]
    [InlineData(PermissionOptions.CommentCreateOnDeleted)]
    [InlineData(PermissionOptions.CommentFull)]
    public async Task HandleRequirementAsync_HaveRequiredPermissions_SucceedResult(string permission)
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
    public async Task HandleRequirementAsync_GameNotDeleted_SucceededResult()
    {
        // Arrange
        SetupAuthContext(new ClaimsPrincipal());
        _unitOfWorkMock.Setup(m => m.Games.GetOneAsync(
            It.IsAny<Expression<Func<Game, bool>>>(),
            It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync(new Game { Deleted = false });

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_GameDeleted_NotSucceededResult()
    {
        // Arrange
        SetupAuthContext(new ClaimsPrincipal());
        _unitOfWorkMock.Setup(m => m.Games.GetOneAsync(
            It.IsAny<Expression<Func<Game, bool>>>(),
            It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
            It.IsAny<bool>()))
            .ReturnsAsync(new Game { Deleted = true });

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.False(_authContext.HasSucceeded);
    }

    private void SetupAuthContext(ClaimsPrincipal principal)
    {
        IEnumerable<IAuthorizationRequirement> requirements = new List<IAuthorizationRequirement> { _requirement };
        _authContext = new AuthorizationHandlerContext(requirements, principal, "test");
    }
}