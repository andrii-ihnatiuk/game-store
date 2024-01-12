using AutoFixture;
using GameStore.API.Controllers;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Role;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class RolesControllerTests
{
    private readonly Mock<IRoleService> _roleServiceMock;
    private readonly RolesController _controller;
    private readonly Fixture _fixture;

    public RolesControllerTests()
    {
        _roleServiceMock = new Mock<IRoleService>();
        _controller = new RolesController(_roleServiceMock.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAllRolesAsync_ReturnsOkObjectResult()
    {
        // Arrange
        var expectedRoles = _fixture.Create<List<RoleBriefDto>>();
        _roleServiceMock.Setup(s => s.GetAllRolesAsync()).ReturnsAsync(expectedRoles);

        // Act
        var result = await _controller.GetAllRolesAsync();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var roles = Assert.IsType<List<RoleBriefDto>>(okResult.Value);
        Assert.Equal(expectedRoles, roles);
    }

    [Fact]
    public async Task GetRoleAsync_GivenValidRoleId_ReturnsOkObjectResult()
    {
        // Arrange
        var expectedRole = _fixture.Create<RoleBriefDto>();
        string roleId = expectedRole.Id;
        _roleServiceMock.Setup(s => s.GetRoleByIdAsync(roleId)).ReturnsAsync(expectedRole);

        // Act
        var result = await _controller.GetRoleAsync(roleId);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var role = Assert.IsType<RoleBriefDto>(okResult.Value);
        Assert.Equal(expectedRole, role);
    }

    [Fact]
    public async Task GetRolePermissionsAsync_GivenValidRoleId_ReturnsOkObjectResult()
    {
        // Arrange
        var expectedPermissions = _fixture.Create<List<string>>();
        var roleId = _fixture.Create<string>();
        _roleServiceMock.Setup(s => s.GetRolePermissionsAsync(roleId)).ReturnsAsync(expectedPermissions);

        // Act
        var result = await _controller.GetRolePermissionsAsync(roleId);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var permissions = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(expectedPermissions, permissions);
    }

    [Fact]
    public void GetAllPermissions_ReturnsOkObjectResult()
    {
        // Act
        var result = _controller.GetAllPermissions();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        string[] permissions = Assert.IsType<string[]>(okResult.Value);
        Assert.Equal(PermissionOptions.AllOptions, permissions);
    }

    [Fact]
    public async Task CreateRoleAsync_GivenValidRole_ReturnsOkObjectResultWithTheRole()
    {
        // Arrange
        var newRole = _fixture.Create<RoleCreateDto>();
        var newlyCreatedRole = _fixture.Create<RoleBriefDto>();
        _roleServiceMock.Setup(s => s.CreateRoleAsync(newRole)).ReturnsAsync(newlyCreatedRole);

        // Act
        var result = await _controller.CreateRoleAsync(newRole);

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var role = Assert.IsType<RoleBriefDto>(okResult.Value);
        Assert.Equal(newlyCreatedRole, role);
    }

    [Fact]
    public async Task UpdateRoleAsync_GivenValidRole_ReturnsNoContentResult()
    {
        // Arrange
        var updatingRole = _fixture.Create<RoleUpdateDto>();
        _roleServiceMock.Setup(s => s.UpdateRoleAsync(updatingRole)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateRoleAsync(updatingRole);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRoleAsync_GivenValidRoleId_ReturnsNoContentResult()
    {
        // Arrange
        var roleId = _fixture.Create<string>();
        _roleServiceMock.Setup(s => s.DeleteRoleAsync(roleId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteRoleAsync(roleId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}