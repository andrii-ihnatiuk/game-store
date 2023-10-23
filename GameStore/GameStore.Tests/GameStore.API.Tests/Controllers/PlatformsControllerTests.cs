using GameStore.API.Controllers;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class PlatformsControllerTests
{
    private readonly PlatformsController _controller;
    private readonly Mock<IPlatformService> _platformService = new();
    private readonly Mock<IValidatorWrapper<PlatformCreateDto>> _platformCreateValidator = new();

    public PlatformsControllerTests()
    {
        _controller = new PlatformsController(_platformService.Object, _platformCreateValidator.Object);
    }

    [Fact]
    public async Task GetPlatform_ReturnsPlatformDto()
    {
        // Arrange
        var platformId = Guid.Empty;
        _platformService.Setup(s => s.GetPlatformByIdAsync(platformId))
            .ReturnsAsync(new PlatformFullDto() { Id = platformId })
            .Verifiable();

        // Act
        var result = await _controller.GetPlatformAsync(platformId);

        // Assert
        _platformService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PlatformFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAllGenres_ReturnsPlatformsBriefDtoList()
    {
        // Arrange
        _platformService.Setup(s => s.GetAllPlatformsAsync())
            .ReturnsAsync(new List<PlatformBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetAllPlatformsAsync();

        // Assert
        _platformService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IList<PlatformBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task PostPlatform_ReturnsCreatedPlatform()
    {
        // Arrange
        var platformCreateDto = new PlatformCreateDto();
        var platformViewDto = new PlatformBriefDto() { Id = Guid.Empty };
        _platformService.Setup(s => s.AddPlatformAsync(platformCreateDto))
            .ReturnsAsync(platformViewDto)
            .Verifiable();

        // Act
        var result = await _controller.PostPlatformAsync(platformCreateDto);

        // Assert
        _platformService.Verify();
        Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(((CreatedAtRouteResult)result).Value, platformViewDto);
    }

    [Fact]
    public async Task UpdatePlatform_ReturnsOk()
    {
        // Arrange
        var dto = new PlatformUpdateDto();
        _platformService.Setup(s => s.UpdatePlatformAsync(dto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.UpdatePlatformAsync(dto);

        // Assert
        _platformService.Verify();
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeletePlatform_ReturnsNoContent()
    {
        // Arrange
        var platformId = Guid.Empty;
        _platformService.Setup(s => s.DeletePlatformAsync(platformId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeletePlatformAsync(platformId);

        // Assert
        _platformService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}