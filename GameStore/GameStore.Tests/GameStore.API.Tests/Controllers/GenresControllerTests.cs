using GameStore.API.Controllers;
using GameStore.Application.Interfaces;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameStore.Tests.GameStore.API.Tests.Controllers;

public class GenresControllerTests
{
    private readonly GenresController _controller;
    private readonly Mock<IGenreService> _genreService = new();
    private readonly Mock<IValidatorWrapper<GenreCreateDto>> _genreCreateValidator = new();
    private readonly Mock<IValidatorWrapper<GenreUpdateDto>> _genreUpdateValidator = new();

    public GenresControllerTests()
    {
        _controller = new GenresController(_genreService.Object, _genreCreateValidator.Object, _genreUpdateValidator.Object);
    }

    [Fact]
    public async Task GetGenre_ReturnsGenreDto()
    {
        // Arrange
        var genreId = Guid.Empty;
        _genreService.Setup(s => s.GetGenreByIdAsync(genreId))
            .ReturnsAsync(new GenreFullDto { Id = genreId })
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
    public async Task GetSubgenresAsync_ReturnsSubgenres()
    {
        // Arrange
        Guid genreId = Guid.Empty;
        _genreService.Setup(s => s.GetSubgenresByParentAsync(genreId))
            .ReturnsAsync(new List<GenreBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetSubgenresAsync(genreId);

        // Assert
        _genreService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IList<GenreBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task GetGamesByGenreAsync_ReturnsGames()
    {
        // Arrange
        Guid genreId = Guid.Empty;
        _genreService.Setup(s => s.GetGamesByGenreId(genreId))
            .ReturnsAsync(new List<GameBriefDto>())
            .Verifiable();

        // Act
        var result = await _controller.GetGamesByGenreAsync(genreId);

        // Assert
        _genreService.Verify();
        Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IList<GameBriefDto>>(((OkObjectResult)result.Result).Value);
    }

    [Fact]
    public async Task PostGenre_ReturnsCreatedGenre()
    {
        // Arrange
        var genreCreateDto = new GenreCreateDto();
        var genreBriefDto = new GenreBriefDto() { Id = Guid.Empty };
        _genreService.Setup(s => s.AddGenreAsync(genreCreateDto))
            .ReturnsAsync(genreBriefDto)
            .Verifiable();

        // Act
        var result = await _controller.PostGenreAsync(genreCreateDto);

        // Assert
        _genreService.Verify();
        Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(((CreatedAtRouteResult)result).Value, genreBriefDto);
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
        var genreId = Guid.Empty;
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