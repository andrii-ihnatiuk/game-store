using GameStore.Data;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Shared.Exceptions;
using GameStore.Tests.Util;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Tests.GameStore.Data.Tests.Repositories;

public class GenericRepositoryTests
{
    private readonly DbSet<Platform> _dbSet;
    private readonly GameStoreDbContext _context;
    private readonly GenericRepository<Platform> _repository;

    public GenericRepositoryTests()
    {
        _context = DatabaseService.CreateSqLiteContext();
        _dbSet = _context.Set<Platform>();
        _repository = new GenericRepository<Platform>(_context);
        ClearDatabase();
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ReturnsEntity()
    {
        // Arrange
        var platform = new Platform { Id = Guid.NewGuid(), Type = "Console" };
        _dbSet.Add(platform);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(platform.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(platform.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityDoesNotExist_ThrowsException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await _repository.GetByIdAsync(nonExistentId));
    }

    [Fact]
    public async Task GetAsync_WhenCalled_ReturnsAllEntities()
    {
        // Arrange
        var platforms = new List<Platform>
        {
            new Platform { Id = Guid.NewGuid(), Type = "Console" },
            new Platform { Id = Guid.NewGuid(), Type = "PC" },
        };
        _dbSet.AddRange(platforms);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync();

        // Assert
        Assert.Equal(platforms.Count, result.Count);
    }

    [Fact]
    public async Task GetAsync_WhenCalledWithPredicate_ReturnsFilteredEntities()
    {
        // Arrange
        var platforms = new List<Platform>
        {
            new Platform { Id = Guid.NewGuid(), Type = "Console" },
            new Platform { Id = Guid.NewGuid(), Type = "PC" },
        };
        _dbSet.AddRange(platforms);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(p => p.Type == "Console");

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_WhenCalledWithOrdering_ReturnsOrderedEntities()
    {
        // Arrange
        var platforms = new List<Platform>
        {
            new Platform { Id = Guid.NewGuid(), Type = "Console" },
            new Platform { Id = Guid.NewGuid(), Type = "PC" },
        };
        _dbSet.AddRange(platforms);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(orderBy: q => q.OrderBy(p => p.Type));

        // Assert
        Assert.Equal("Console", result.First().Type);
        Assert.Equal("PC", result.Last().Type);
    }

    [Fact]
    public async Task GetAsync_WhenCalledIncludingRelatedEntities_ReturnsEntitiesWithIncludedRelatedEntities()
    {
        // Arrange
        var game = new Game()
        {
            Id = Guid.NewGuid(),
            Alias = string.Empty,
            Name = "Among Us",
            Price = 10,
            UnitInStock = 10,
            Discontinued = false,
        };
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        var platform = new Platform
        {
            Id = Guid.NewGuid(),
            Type = "Console",
            PlatformGames = new List<GamePlatform>
            {
                new GamePlatform() { GameId = game.Id },
            },
        };

        _dbSet.Add(platform);
        await _context.SaveChangesAsync();

        // Act
        // Include related entities
        var result =
            await _repository.GetAsync(include: x => x.Include(p => p.PlatformGames).ThenInclude(gp => gp.Game));

        // Assert
        Assert.NotEmpty(result);
        Assert.True(result.First().PlatformGames.Count == 1);
        Assert.Equal("Among Us", result.First().PlatformGames.ToList()[0].Game.Name);
    }

    [Fact]
    public async Task GetAsync_WhenCalledWithNoTracking_ResultHasNoTracking()
    {
        // Arrange
        var platform = new Platform
        {
            Id = Guid.NewGuid(),
            Type = "Console",
        };

        _dbSet.Add(platform);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        // Enable NoTracking
        var result = await _repository.GetAsync(noTracking: true);

        // Assert
        Assert.NotEmpty(result);

        // Ensure no change tracking for loaded entities when noTracking is set to true.
        var entries = _context.ChangeTracker.Entries<Platform>();
        Assert.DoesNotContain(entries, e => e.Entity.Id == result.First().Id);
    }

    [Fact]
    public async Task GetOneAsync_WhenEntityExists_ReturnsEntity()
    {
        // Arrange
        var platform = new Platform { Id = Guid.NewGuid(), Type = "Console" };
        _dbSet.Add(platform);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOneAsync(p => p.Id == platform.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(platform.Id, result.Id);
    }

    [Fact]
    public async Task GetOneAsync_WhenEntityDoesNotExist_ThrowsException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            async () => await _repository.GetOneAsync(p => p.Id == nonExistentId));
    }

    [Fact]
    public async Task GetOneAsync_WhenEntityExistsWithRelatedEntities_ReturnsEntityWithRelatedEntities()
    {
        // Arrange
        var game = new Game()
        {
            Id = Guid.NewGuid(),
            Alias = string.Empty,
            Name = "Among Us",
            Price = 10,
            UnitInStock = 10,
            Discontinued = false,
        };
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        var platform = new Platform
        {
            Id = Guid.NewGuid(),
            Type = "Console",
            PlatformGames = new List<GamePlatform>
            {
                new GamePlatform() { GameId = game.Id },
            },
        };
        _dbSet.Add(platform);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOneAsync(
            p => p.Id == platform.Id,
            include: x => x.Include(p => p.PlatformGames).ThenInclude(gp => gp.Game));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.PlatformGames.Count == 1);
        Assert.Equal("Among Us", result.PlatformGames.ToList()[0].Game.Name);
    }

    [Fact]
    public async Task AddAsync_WhenValidEntity_IsAddedSuccessfully()
    {
        // Arrange
        var platform = new Platform { Id = Guid.NewGuid(), Type = "Console" };

        // Act
        await _repository.AddAsync(platform);
        await _context.SaveChangesAsync();

        // Assert
        Assert.True(_dbSet.Any(p => p.Id == platform.Id));
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityExists_DeletesEntity()
    {
        // Arrange
        var platform = new Platform { Id = Guid.NewGuid(), Type = "Console" };
        _dbSet.Add(platform);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(platform.Id);
        await _context.SaveChangesAsync();

        // Assert
        Assert.False(_dbSet.Any(p => p.Id == platform.Id));
    }

    [Fact]
    public async Task DeleteAsync_WhenEntityDoesNotExist_DoesNotThrowException()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        await _repository.DeleteAsync(nonExistentId);
        await _context.SaveChangesAsync();

        // Assert doesn't throw an exception
    }

    [Fact]
    public async Task Update_WhenEntityExists_UpdatesEntity()
    {
        // Arrange
        var platform = new Platform { Id = Guid.NewGuid(), Type = "Console" };
        _dbSet.Add(platform);
        await _context.SaveChangesAsync();

        platform.Type = "PC";
        _repository.Update(platform);
        await _context.SaveChangesAsync();

        // Act
        var updatedEntity = await _dbSet.FindAsync(platform.Id);

        // Assert
        Assert.Equal("PC", updatedEntity.Type);
    }

    [Fact]
    public async Task Update_WhenEntityNotAttached_AttachesEntity()
    {
        // Arrange
        var platform = new Platform { Id = Guid.NewGuid(), Type = "PC" };

        // Act
        _repository.Update(platform);

        // Assert
        var updatedEntity = await _dbSet.FindAsync(platform.Id);
        Assert.NotNull(updatedEntity);
        Assert.True(_dbSet.Entry(updatedEntity!).State == EntityState.Modified);
        Assert.Equal("PC", updatedEntity.Type);
    }

    [Fact]
    public async Task ExistsAsync_WhenEntityExists_ReturnsTrue()
    {
        // Arrange
        var platform = new Platform { Id = Guid.NewGuid(), Type = "Console" };
        _dbSet.Add(platform);
        await _context.SaveChangesAsync();

        // Act
        bool exists = await _repository.ExistsAsync(p => p.Id == platform.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_WhenEntityDoesNotExist_ReturnsFalse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        bool exists = await _repository.ExistsAsync(p => p.Id == nonExistentId);

        // Assert
        Assert.False(exists);
    }

    private void ClearDatabase()
    {
        _dbSet.RemoveRange(_dbSet);
        _context.SaveChanges();
    }
}