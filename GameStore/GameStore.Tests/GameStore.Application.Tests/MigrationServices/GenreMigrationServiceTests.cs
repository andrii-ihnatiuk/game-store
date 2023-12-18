using System.Linq.Expressions;
using AutoMapper;
using GameStore.Application.Services.Migration;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Shared.DTOs.Genre;
using Moq;
using Northwind.Data.Entities;
using Northwind.Data.Interfaces;

namespace GameStore.Tests.GameStore.Application.Tests.MigrationServices;

public class GenreMigrationServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMongoUnitOfWork> _mockMongoUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GenreMigrationService _service;

    public GenreMigrationServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMongoUnitOfWork = new Mock<IMongoUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _service = new GenreMigrationService(_mockUnitOfWork.Object, _mockMongoUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task MigrateOnUpdateAsync_WhenEntityRequiresMigration_MigratesGenre()
    {
        // Arrange
        const string genreId = "genreId";
        var genreDto = new GenreUpdateDto { Genre = new GenreUpdateInnerDto { Name = "name", Id = genreId } };

        _mockMongoUnitOfWork
            .Setup(u => u.Categories.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(true);

        _mockUnitOfWork
            .Setup(u => u.Genres.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(false);

        var migratedGenre = new Genre();
        _mockMapper
            .Setup(m => m.Map<Genre>(genreDto))
            .Returns(migratedGenre);

        _mockUnitOfWork
            .Setup(u => u.Genres.AddAsync(migratedGenre))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(u => u.SaveAsync())
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.MigrateOnUpdateAsync(genreDto);

        // Assert
        Assert.Equal(genreDto, result);
        _mockUnitOfWork.Verify(u => u.Genres.AddAsync(migratedGenre), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task MigrateOnUpdateAsync_WhenEntityDoesNotRequireMigration_ReturnsDto()
    {
        // Arrange
        var genreId = Guid.Empty.ToString();
        var genreDto = new GenreUpdateDto { Genre = new GenreUpdateInnerDto { Name = "name", Id = genreId } };

        _mockUnitOfWork
            .Setup(u => u.SaveAsync())
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.MigrateOnUpdateAsync(genreDto);

        // Assert
        Assert.Equal(genreDto, result);
        _mockMongoUnitOfWork.Verify(u => u.Categories.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.Genres.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Never);
        _mockMapper.Verify(m => m.Map<Genre>(genreDto), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task MigrateOnCreateAsync_WhenParentMigrationRequired_MigrateParent()
    {
        // Arrange
        const string genreId = "genreId";
        var genreDto = new GenreCreateDto { Genre = new GenreCreateInnerDto { Name = "name", ParentGenreId = genreId } };

        var category = new Category()
        {
            Id = "category-id",
            CategoryName = "name",
            Picture = "picture",
        };
        _mockMongoUnitOfWork.Setup(u => u.Categories.GetOneAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(category);
        _mockMongoUnitOfWork.Setup(u => u.Categories.ExistsAsync(It.IsAny<Expression<Func<Category, bool>>>()))
            .ReturnsAsync(false);

        var id = Guid.NewGuid();
        _mockUnitOfWork.Setup(u => u.Genres.AddAsync(It.IsAny<Genre>()))
            .Callback<Genre>(genre => genre.Id = id)
            .Returns(Task.CompletedTask);
        _mockUnitOfWork
            .Setup(u => u.SaveAsync())
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.MigrateOnCreateAsync(genreDto);

        // Assert
        Assert.Equal(genreDto, result);
        Assert.Equal(genreDto.Genre.ParentGenreId, id.ToString());
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task MigrateOnCreateAsync_WhenParentMigrationNotRequired_ReturnDto()
    {
        // Arrange
        var genreId = Guid.Empty.ToString();
        var genreDto = new GenreCreateDto { Genre = new GenreCreateInnerDto { Name = "name", ParentGenreId = genreId } };

        _mockUnitOfWork
            .Setup(u => u.SaveAsync())
            .Returns(Task.FromResult(1));

        // Act
        var result = await _service.MigrateOnCreateAsync(genreDto);

        // Assert
        Assert.Equal(genreDto, result);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
}