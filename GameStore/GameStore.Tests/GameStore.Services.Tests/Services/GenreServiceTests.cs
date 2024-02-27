using System.Linq.Expressions;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.Exceptions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class GenreServiceTests
{
    private const string GenreName = "test";
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly CoreGenreService _service;

    public GenreServiceTests()
    {
        _service = new CoreGenreService(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetGenreByIdAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        var id = Guid.Empty;
        var genre = new Genre { Id = id };
        _unitOfWork.Setup(uow => uow.Genres.GetOneAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(genre);

        _mapper.Setup(m => m.Map<Genre, GenreFullDto>(genre))
            .Returns(new GenreFullDto());

        // Act
        await _service.GetGenreByIdAsync(id.ToString(), string.Empty);

        // Assert
        _unitOfWork.Verify(
            uow => uow.Genres.GetOneAsync(
                g => g.Id == id,
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()),
            Times.Once);
    }

    [Fact]
    public async Task GetSubgenresByParentAsync_ReturnsGenres()
    {
        // Arrange
        var parentId = Guid.Empty;
        var subGenresData = new List<Genre> { new(), new() };
        _unitOfWork.Setup(uow => uow.Genres.GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(subGenresData);

        _mapper.Setup(m => m.Map<IList<GenreBriefDto>>(subGenresData))
            .Returns(new List<GenreBriefDto> { new(), new() });

        // Act
        var subGenres = await _service.GetSubgenresByParentAsync(parentId.ToString(), string.Empty);

        // Assert
        Assert.NotNull(subGenres);
        Assert.IsAssignableFrom<IList<GenreBriefDto>>(subGenres);
    }

    [Fact]
    public async Task GetGamesByGenreId_ReturnsGames()
    {
        // Arrange
        var genreId = Guid.Empty;
        var gamesGenres = new List<GameGenre> { new() { Game = new Game() }, new() { Game = new Game() } };
        var games = gamesGenres.Select(gg => gg.Game).ToList();
        _unitOfWork.Setup(uow => uow.GamesGenres.GetAsync(
                It.IsAny<Expression<Func<GameGenre, bool>>>(),
                It.IsAny<Func<IQueryable<GameGenre>, IOrderedQueryable<GameGenre>>>(),
                It.IsAny<Func<IQueryable<GameGenre>, IIncludableQueryable<GameGenre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(gamesGenres);

        _mapper.Setup(m => m.Map<IList<GameBriefDto>>(games))
            .Returns(games.Select(game => new GameBriefDto()).ToList());

        // Act
        var gamesDto = await _service.GetGamesByGenreIdAsync(genreId.ToString(), string.Empty);

        // Assert
        Assert.NotNull(gamesDto);
        Assert.IsAssignableFrom<IList<GameBriefDto>>(gamesDto);
    }

    [Fact]
    public async Task GetAllGenresAsync_ReturnsGenres()
    {
        // Arrange
        var genresData = new List<Genre> { new(), new() };
        _unitOfWork.Setup(uow => uow.Genres.GetAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IOrderedQueryable<Genre>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(genresData)
            .Verifiable();

        _mapper.Setup(m => m.Map<IList<GenreBriefDto>>(genresData))
            .Returns(new List<GenreBriefDto> { new(), new() });

        // Act
        var genres = await _service.GetAllGenresAsync(string.Empty);

        // Assert
        _unitOfWork.Verify();
        Assert.Equal(genresData.Count, genres.Count);
    }

    [Fact]
    public async Task AddGenreAsync_AllOk_CallsRepository()
    {
        // Arrange
        var genreCreateDto = new GenreCreateDto();
        var genre = new Genre() { Id = Guid.Empty, Name = GenreName };

        _mapper.Setup(m => m.Map<Genre>(genreCreateDto))
            .Returns(genre);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == GenreName))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Genres.AddAsync(genre))
            .Returns(Task.CompletedTask);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        _mapper.Setup(m => m.Map<GenreFullDto>(genre))
            .Returns(new GenreFullDto());

        // Act
        await _service.AddGenreAsync(genreCreateDto);

        // Assert
        _unitOfWork.Verify(uow => uow.Genres.AddAsync(genre), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddGenreAsync_WhenDuplicateName_ThrowsEntityAlreadyExistsException()
    {
        // Arrange
        var genreCreateDto = new GenreCreateDto();
        var genre = new Genre() { Name = GenreName };

        _mapper.Setup(m => m.Map<Genre>(genreCreateDto))
            .Returns(genre);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == GenreName))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.AddGenreAsync(genreCreateDto));
    }

    [Fact]
    public async Task AddGenreAsync_WhenParentGenreDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var genreCreateDto = new GenreCreateDto();
        var genre = new Genre() { ParentGenreId = Guid.Empty, Name = GenreName };

        _mapper.Setup(m => m.Map<Genre>(genreCreateDto))
            .Returns(genre);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == GenreName))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.AddGenreAsync(genreCreateDto));
    }

    [Fact]
    public async Task UpdateGenreAsync_AllOk_CallsRepository()
    {
        // Arrange
        var genreUpdateDto = new GenreUpdateDto { Genre = new GenreUpdateInnerDto { Id = Guid.Empty.ToString() } };
        var existingGenre = new Genre() { Id = Guid.Empty };

        _unitOfWork.Setup(uow => uow.Genres.GetOneAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingGenre)
            .Verifiable(Times.Once);

        var updatedGenre = new Genre() { Id = Guid.Empty, Name = GenreName };
        _mapper.Setup(m => m.Map(genreUpdateDto, existingGenre))
            .Returns(updatedGenre);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == GenreName))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1)
            .Verifiable(Times.Once);

        // Act
        await _service.UpdateGenreAsync(genreUpdateDto);

        // Assert
        _unitOfWork.Verify();
    }

    [Fact]
    public async Task UpdateGenreAsync_WhenDuplicateName_ThrowsEntityAlreadyExistsException()
    {
        // Arrange
        const string updatedName = "updated-but-already-exists";
        var genreUpdateDto = new GenreUpdateDto { Genre = new GenreUpdateInnerDto { Id = Guid.Empty.ToString(), Name = updatedName } };

        var existingGenre = new Genre() { Id = Guid.Empty, Name = GenreName };
        _unitOfWork.Setup(uow => uow.Genres.GetOneAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingGenre)
            .Verifiable(Times.Once);

        _mapper.Setup(m => m.Map(genreUpdateDto, existingGenre))
            .Returns(new Genre() { Id = Guid.Empty, Name = updatedName });

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == updatedName))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.UpdateGenreAsync(genreUpdateDto));
    }

    [Fact]
    public async Task UpdateGenreAsync_ParentGenreDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var genreUpdateDto = new GenreUpdateDto
        {
            Genre = new GenreUpdateInnerDto { Id = Guid.Empty.ToString(), ParentGenreId = null },
            Culture = "en",
        };

        var existingGenre = new Genre() { Id = Guid.Empty };
        _unitOfWork.Setup(uow => uow.Genres.GetOneAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(existingGenre)
            .Verifiable(Times.Once);

        var updatedGenre = new Genre() { Id = Guid.Empty, ParentGenreId = Guid.Empty };
        _mapper.Setup(m => m.Map(genreUpdateDto as object, existingGenre))
            .Callback(() => existingGenre.ParentGenreId = Guid.Empty);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.UpdateGenreAsync(genreUpdateDto));
    }

    [Fact]
    public async Task DeleteGenreAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        _unitOfWork.Setup(uow => uow.Genres.GetOneAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new Genre { Id = Guid.Empty });

        _unitOfWork.Setup(uow => uow.Genres.DeleteAsync(It.IsAny<object>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

        // Act
        await _service.DeleteGenreAsync(Guid.Empty.ToString());

        // Assert
        _unitOfWork.Verify(uow => uow.Genres.DeleteAsync(It.IsAny<object>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}