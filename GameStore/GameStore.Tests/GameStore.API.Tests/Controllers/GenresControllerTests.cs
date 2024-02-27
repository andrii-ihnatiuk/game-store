﻿using GameStore.API.Controllers;
using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class GenresControllerTests
{
    private readonly GenresController _controller;
    private readonly Mock<IGenreFacadeService> _genreFacadeService = new();
    private readonly Mock<IValidatorWrapper<GenreCreateDto>> _genreCreateValidator = new();
    private readonly Mock<IValidatorWrapper<GenreUpdateDto>> _genreUpdateValidator = new();

    public GenresControllerTests()
    {
        _controller = new GenresController(_genreFacadeService.Object, _genreCreateValidator.Object, _genreUpdateValidator.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext(),
            },
        };

        _controller.HttpContext.Features.Set<IRequestCultureFeature>(
            new RequestCultureFeature(new RequestCulture("en", "en"), null));
    }

    [Fact]
    public async Task GetGenre_ReturnsGenreDto()
    {
        // Arrange
        var genreId = Guid.Empty.ToString();
        _genreFacadeService.Setup(s => s.GetGenreByIdAsync(genreId, It.IsAny<string>()))
            .ReturnsAsync(new GenreFullDto { Id = genreId })
            .Verifiable();

        // Act
        var result = await _controller.GetGenreAsync(genreId);

        // Assert
        _genreFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GenreFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAllGenres_ReturnsGenresBriefDtoList()
    {
        // Arrange
        _genreFacadeService.Setup(s => s.GetAllGenresAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<GenreBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetAllGenresAsync();

        // Assert
        _genreFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IList<GenreBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetSubgenresAsync_ReturnsSubgenres()
    {
        // Arrange
        var genreId = Guid.Empty.ToString();
        _genreFacadeService.Setup(s => s.GetSubgenresByParentAsync(genreId, It.IsAny<string>()))
            .ReturnsAsync(new List<GenreBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetSubgenresAsync(genreId);

        // Assert
        _genreFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IList<GenreBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetGamesByGenreAsync_ReturnsGames()
    {
        // Arrange
        var genreId = Guid.Empty.ToString();
        _genreFacadeService.Setup(s => s.GetGamesByGenreIdAsync(genreId, It.IsAny<string>()))
            .ReturnsAsync(new List<GameBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetGamesByGenreAsync(genreId);

        // Assert
        _genreFacadeService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IList<GameBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task PostGenre_ReturnsCreatedGenre()
    {
        // Arrange
        var genreCreateDto = new GenreCreateDto();
        var genreBriefDto = new GenreBriefDto() { Id = Guid.Empty.ToString() };
        _genreFacadeService.Setup(s => s.AddGenreAsync(genreCreateDto))
            .ReturnsAsync(genreBriefDto)
            .Verifiable();

        // Act
        var result = await _controller.PostGenreAsync(genreCreateDto);

        // Assert
        _genreFacadeService.Verify();
        Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(((CreatedAtRouteResult)result).Value, genreBriefDto);
    }

    [Fact]
    public async Task UpdateGenre_ReturnsOk()
    {
        // Arrange
        var dto = new GenreUpdateDto();
        _genreFacadeService.Setup(s => s.UpdateGenreAsync(dto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.UpdateGenreAsync(dto);

        // Assert
        _genreFacadeService.Verify();
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteGenre_ReturnsNoContent()
    {
        // Arrange
        var genreId = Guid.Empty.ToString();
        _genreFacadeService.Setup(s => s.DeleteGenreAsync(genreId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeleteGenreAsync(genreId);

        // Assert
        _genreFacadeService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}