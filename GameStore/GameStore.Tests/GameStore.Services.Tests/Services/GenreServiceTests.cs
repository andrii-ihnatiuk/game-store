using System.Linq.Expressions;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Genre;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class GenreServiceTests
{
    private const string GenreName = "test";
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly GenreService _service;

    public GenreServiceTests()
    {
        _service = new GenreService(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetGenreByIdAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        const long id = 1;
        var genre = new Genre { Id = id };
        _unitOfWork.Setup(uow => uow.Genres.GetOneAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(genre);

        _mapper.Setup(m => m.Map<Genre, GenreFullDto>(genre))
            .Returns(new GenreFullDto());

        // Act
        await _service.GetGenreByIdAsync(id);

        // Assert
        _unitOfWork.Verify(
            uow => uow.Genres.GetOneAsync(
                g => g.Id == id,
                It.IsAny<Func<IQueryable<Genre>, IIncludableQueryable<Genre, object>>>(),
                It.IsAny<bool>()),
            Times.Once);
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
        var genres = await _service.GetAllGenresAsync();

        // Assert
        _unitOfWork.Verify();
        Assert.Equal(genresData.Count, genres.Count);
    }

    [Fact]
    public async Task AddGenreAsync_AllOk_CallsRepository()
    {
        // Arrange
        var genreCreateDto = new GenreCreateDto();
        var genre = new Genre() { Id = 1, Name = GenreName };

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
        var genre = new Genre() { ParentGenreId = 0, Name = GenreName };

        _mapper.Setup(m => m.Map<Genre>(genreCreateDto))
            .Returns(genre);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == GenreName))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Id == genre.ParentGenreId))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.AddGenreAsync(genreCreateDto));
    }

    [Fact]
    public async Task UpdateGenreAsync_AllOk_CallsRepository()
    {
        // Arrange
        var genreUpdateDto = new GenreUpdateDto() { GenreId = 1 };
        var existingGenre = new Genre() { Id = 1 };

        _unitOfWork.Setup(uow => uow.Genres.GetByIdAsync(genreUpdateDto.GenreId))
            .ReturnsAsync(existingGenre);

        var updatedGenre = new Genre() { Id = 1, Name = GenreName };
        _mapper.Setup(m => m.Map(genreUpdateDto, existingGenre))
            .Returns(updatedGenre);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == GenreName))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        // Act
        await _service.UpdateGenreAsync(genreUpdateDto);

        // Assert
        _unitOfWork.Verify(uow => uow.Genres.GetByIdAsync(genreUpdateDto.GenreId), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateGenreAsync_WhenDuplicateName_ThrowsEntityAlreadyExistsException()
    {
        // Arrange
        const string updatedName = "updated-but-already-exists";
        var genreUpdateDto = new GenreUpdateDto() { GenreId = 1, Name = updatedName };

        var existingGenre = new Genre() { Id = 1, Name = GenreName };
        _unitOfWork.Setup(uow => uow.Genres.GetByIdAsync(genreUpdateDto.GenreId))
            .ReturnsAsync(existingGenre);

        _mapper.Setup(m => m.Map(genreUpdateDto, existingGenre))
            .Returns(new Genre() { Id = 1, Name = updatedName });

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Name == updatedName))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.UpdateGenreAsync(genreUpdateDto));
    }

    [Fact]
    public async Task UpdateGenreAsync_ParentGenreDoesNotExist_ThrowsForeignKeyException()
    {
        // Arrange
        var genreUpdateDto = new GenreUpdateDto() { GenreId = 1, ParentGenreId = 2 };

        var existingGenre = new Genre() { Id = 1 };
        _unitOfWork.Setup(uow => uow.Genres.GetByIdAsync(genreUpdateDto.GenreId))
            .ReturnsAsync(existingGenre);

        var updatedGenre = new Genre() { Id = 1, ParentGenreId = 2 };
        _mapper.Setup(m => m.Map(genreUpdateDto, existingGenre))
            .Returns(updatedGenre);

        _unitOfWork.Setup(uow => uow.Genres.ExistsAsync(g => g.Id == updatedGenre.ParentGenreId))
            .ReturnsAsync(false);

        // Act and Assert
        await Assert.ThrowsAsync<ForeignKeyException>(() => _service.UpdateGenreAsync(genreUpdateDto));
    }

    [Fact]
    public async Task DeleteGenreAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        const long genreId = 1;
        _unitOfWork.Setup(uow => uow.Genres.DeleteAsync(genreId)).Returns(Task.CompletedTask);
        _unitOfWork.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

        // Act
        await _service.DeleteGenreAsync(genreId);

        // Assert
        _unitOfWork.Verify(uow => uow.Genres.DeleteAsync(genreId), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}