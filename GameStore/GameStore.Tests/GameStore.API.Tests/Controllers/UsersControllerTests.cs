using AutoFixture;
using GameStore.API.Controllers;
using GameStore.Application.Interfaces.Util;
using GameStore.Services.Interfaces;
using GameStore.Services.Interfaces.Authentication;
using GameStore.Shared.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<ILoginService> _loginServiceMock = new();
    private readonly Mock<ILoginServiceResolver> _loginServiceResolverMock = new();
    private readonly UsersController _controller;
    private readonly Fixture _fixture;

    public UsersControllerTests()
    {
        _loginServiceResolverMock.Setup(s => s.Resolve(It.IsAny<UserLoginModel>()))
            .Returns(_loginServiceMock.Object);
        _controller = new UsersController(_userServiceMock.Object, _loginServiceResolverMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var expectedUsers = _fixture.Create<List<UserBriefDto>>();
        _userServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(expectedUsers);

        // Act
        var result = await _controller.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var users = Assert.IsType<List<UserBriefDto>>(okResult.Value);
        Assert.Equal(expectedUsers, users);
    }

    [Fact]
    public async Task GetUserAsync_GivenValidUserId_ReturnsOkObjectResult()
    {
        // Arrange
        var expectedUser = _fixture.Create<UserBriefDto>();
        var userId = _fixture.Create<string>();
        _userServiceMock.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);

        // Act
        var result = await _controller.GetUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var user = Assert.IsType<UserBriefDto>(okResult.Value);
        Assert.Equal(expectedUser, user);
    }

    [Fact]
    public void CheckAccess_ReturnsOkObjectResult()
    {
        // Arrange && Act
        var result = _controller.CheckAccess();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True(okResult.Value is true);
    }

    [Fact]
    public async Task RegisterUserAsync_GivenValidUser_ReturnsOkObjectResultWithUser()
    {
        // Arrange
        var newUser = _fixture.Create<UserRegistrationDto>();
        var registeredUser = _fixture.Create<UserBriefDto>();
        _userServiceMock.Setup(s => s.RegisterUserAsync(newUser)).ReturnsAsync(registeredUser);

        // Act
        var result = await _controller.RegisterUserAsync(newUser);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var user = Assert.IsType<UserBriefDto>(okResult.Value);
        Assert.Equal(registeredUser, user);
    }

    [Fact]
    public async Task LoginAsync_GivenValidLoginDto_ReturnsOkObjectResultWithToken()
    {
        // Arrange
        var loginDto = _fixture.Create<UserLoginDto>();
        var token = _fixture.Create<string>();
        _loginServiceMock.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(token);

        // Act
        var result = await _controller.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenResult = okResult.Value;
        Assert.Equal(token, tokenResult.GetType().GetProperty("token").GetValue(tokenResult));
    }

    [Fact]
    public async Task UpdateUserAsync_GivenValidUser_ReturnsNoContentResult()
    {
        // Arrange
        var userUpdateDto = _fixture.Create<UserUpdateDto>();
        _userServiceMock.Setup(s => s.UpdateUserAsync(userUpdateDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateUserAsync(userUpdateDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteUserAsync_GivenValidUserId_ReturnsNoContentResult()
    {
        // Arrange
        var userId = _fixture.Create<string>();
        _userServiceMock.Setup(s => s.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NoContentResult>(result);
    }
}