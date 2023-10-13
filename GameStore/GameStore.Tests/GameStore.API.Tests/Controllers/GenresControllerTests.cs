﻿using GameStore.API.Controllers;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Genre;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class GenresControllerTests
{
    private readonly GenresController _controller;
    private readonly Mock<IGenreService> _genreService = new();

    public GenresControllerTests()
    {
        _controller = new GenresController(_genreService.Object);
    }

    [Fact]
    public async Task GetGenre_ReturnsGenreDto()
    {
        // Arrange
        const long genreId = 1;
        _genreService.Setup(s => s.GetGenreByIdAsync(genreId))
            .ReturnsAsync(new GenreFullDto { GenreId = genreId })
            .Verifiable();

        // Act
        var result = await _controller.GetGenreAsync(genreId);

        // Assert
        _genreService.Verify();
        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<GenreFullDto>(((OkObjectResult)result).Value);
    }

    [Fact]
    public async Task GetAllGenres_ReturnsGenresBriefDtoList()
    {
        // Arrange
        _genreService.Setup(s => s.GetAllGenresAsync())
            .ReturnsAsync(new List<GenreBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetAllGenresAsync();

        // Assert
        _genreService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IList<GenreBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task PostGenre_ReturnsCreatedGenre()
    {
        // Arrange
        var genreCreateDto = new GenreCreateDto();
        var genreFullDto = new GenreFullDto { GenreId = 1 };
        _genreService.Setup(s => s.AddGenreAsync(genreCreateDto))
            .ReturnsAsync(genreFullDto)
            .Verifiable();

        // Act
        var result = await _controller.PostGenreAsync(genreCreateDto);

        // Assert
        _genreService.Verify();
        Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(((CreatedAtRouteResult)result).Value, genreFullDto);
    }

    [Fact]
    public async Task UpdateGenre_ReturnsOk()
    {
        // Arrange
        var dto = new GenreUpdateDto();
        _genreService.Setup(s => s.UpdateGenreAsync(dto))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.UpdateGenreAsync(dto);

        // Assert
        _genreService.Verify();
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteGenre_ReturnsNoContent()
    {
        // Arrange
        const long genreId = 1;
        _genreService.Setup(s => s.DeleteGenreAsync(genreId))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _controller.DeleteGenreAsync(genreId);

        // Assert
        _genreService.Verify();
        Assert.IsType<NoContentResult>(result);
    }
}