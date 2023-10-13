﻿using GameStore.API.Controllers;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Platform;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class PlatformsControllerTests
{
    private readonly PlatformsController _controller;
    private readonly Mock<IPlatformService> _platformService = new();

    public PlatformsControllerTests()
    {
        _controller = new PlatformsController(_platformService.Object);
    }

    [Fact]
    public async Task GetPlatform_ReturnsPlatformDto()
    {
        // Arrange
        const long platformId = 1;
        _platformService.Setup(s => s.GetPlatformByIdAsync(platformId))
            .ReturnsAsync(new PlatformFullDto() { PlatformId = platformId })
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
        var platformViewDto = new PlatformFullDto() { PlatformId = 1 };
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
        const long platformId = 1;
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