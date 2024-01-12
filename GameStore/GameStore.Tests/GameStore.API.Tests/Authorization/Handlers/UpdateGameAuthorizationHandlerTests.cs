using System.Linq.Expressions;
using System.Security.Claims;
using AutoFixture;
using GameStore.API.Authorization.Handlers;
using GameStore.API.Authorization.Requirements;
using GameStore.Application.Interfaces;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Authorization.Handlers;

public class UpdateGameAuthorizationHandlerTests
{
    private readonly Mock<IGameFacadeService> _gameFacadeMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly UpdateGameAuthorizationHandler _handler;
    private readonly IAuthorizationRequirement _requirement;
    private readonly Fixture _fixture;
    private AuthorizationHandlerContext _authContext;

    public UpdateGameAuthorizationHandlerTests()
    {
        _handler = new UpdateGameAuthorizationHandler(_gameFacadeMock.Object, _unitOfWorkMock.Object);
        _requirement = new CanUpdateGameRequirement();
        _fixture = new Fixture();
    }

    [Fact]
    public async Task HandleRequirementAsync_HasFullPermissions_SucceedResult()
    {
        // Arrange
        var principal = GetPrincipalWithPermissions(PermissionOptions.GameFull);
        SetupAuthContext(principal);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasSucceeded);
    }

    [Theory]
    [InlineData(true, PermissionOptions.GameUpdateDeleted)]
    [InlineData(false, PermissionOptions.GameUpdate)]
    public async Task HandleRequirementAsync_HasPermissionsToUpdate_SucceedResult(bool isDeleted, string permission)
    {
        // Arrange
        var game = _fixture.Build<GameFullDto>().With(g => g.Deleted, isDeleted).Create();
        _gameFacadeMock.Setup(s => s.GetGameByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(game);

        var principal = GetPrincipalWithPermissions(permission);
        var resource = _fixture.Create<GameUpdateDto>();
        SetupAuthContext(principal, resource);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasSucceeded);
        Assert.False(_authContext.HasFailed);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task HandleRequirementAsync_HasNoPermissionsToUpdate_FailedResult(bool isDeleted)
    {
        // Arrange
        var fixture = new Fixture();
        var game = fixture.Build<GameFullDto>().With(g => g.Deleted, isDeleted).Create();
        _gameFacadeMock.Setup(s => s.GetGameByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(game);

        var resource = fixture.Create<GameUpdateDto>();
        SetupAuthContext(new ClaimsPrincipal(), resource);

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.False(_authContext.HasSucceeded);
        Assert.True(_authContext.HasFailed);
    }

    [Fact]
    public async Task HandleRequirementAsync_PublisherHasPermissionToUpdateOwnGame_SucceedResult()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaimTypes.Permission, PermissionOptions.GameUpdateOwn),
            new(ClaimTypes.Role, Roles.Publisher),
            new(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
        }));
        SetupAuthContext(principal, _fixture.Create<GameUpdateDto>());

        var publisherId = Guid.Empty.ToString();
        var game = _fixture.Build<GameFullDto>().With(g => g.PublisherId, publisherId).Create();
        _gameFacadeMock.Setup(s => s.GetGameByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(game);

        _unitOfWorkMock.Setup(s => s.Publishers.GetOneAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new Publisher { Id = Guid.Parse(publisherId) });

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_PublisherIdMismatch_FailedResult()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaimTypes.Permission, PermissionOptions.GameUpdateOwn),
            new(ClaimTypes.Role, Roles.Publisher),
            new(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
        }));
        SetupAuthContext(principal, _fixture.Create<GameUpdateDto>());

        var game = _fixture.Build<GameFullDto>().With(g => g.PublisherId, "wrong id").Create();
        _gameFacadeMock.Setup(s => s.GetGameByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(game);

        _unitOfWorkMock.Setup(s => s.Publishers.GetOneAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new Publisher { Id = Guid.Empty });

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasFailed);
    }

    [Fact]
    public async Task HandleRequirementAsync_PublisherWithAccountIdNotFound_FailedResult()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaimTypes.Permission, PermissionOptions.GameUpdateOwn),
            new(ClaimTypes.Role, Roles.Publisher),
            new(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
        }));
        SetupAuthContext(principal, _fixture.Create<GameUpdateDto>());

        _gameFacadeMock.Setup(s => s.GetGameByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(_fixture.Create<GameFullDto>());

        _unitOfWorkMock.Setup(s => s.Publishers.GetOneAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object>>>(),
                It.IsAny<bool>()))
            .ThrowsAsync(new EntityNotFoundException());

        // Act
        await _handler.HandleAsync(_authContext);

        // Assert
        Assert.True(_authContext.HasFailed);
    }

    private void SetupAuthContext(ClaimsPrincipal principal, GameUpdateDto? dto = null)
    {
        IEnumerable<IAuthorizationRequirement> requirements = new List<IAuthorizationRequirement> { _requirement };
        _authContext = new AuthorizationHandlerContext(requirements, principal, dto ?? new GameUpdateDto());
    }

    private static ClaimsPrincipal GetPrincipalWithPermissions(params string[] permissions)
    {
        ICollection<Claim> claims = permissions.Select(permission =>
            new Claim(CustomClaimTypes.Permission, permission)).ToList();
        return new ClaimsPrincipal(new ClaimsIdentity(claims));
    }
}