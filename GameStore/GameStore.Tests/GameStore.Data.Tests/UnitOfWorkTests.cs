using GameStore.Data;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Tests.Util;
using Moq;

namespace GameStore.Tests.GameStore.Data.Tests;

public class UnitOfWorkTests : IDisposable
{
    private readonly Mock<IGameRepository> _gameRepoMock = new();
    private readonly Mock<IGenericRepository<Genre>> _genreRepoMock = new();
    private readonly Mock<IGenericRepository<Platform>> _platformRepoMock = new();
    private readonly Mock<IGenericRepository<Publisher>> _publisherRepoMock = new();
    private readonly Mock<IGenericRepository<GameGenre>> _gamesGenresRepoMock = new();
    private readonly Mock<IGenericRepository<GamePlatform>> _gamesPlatformsRepoMock = new();
    private readonly Mock<IOrderRepository> _ordersRepoMock = new();
    private readonly Mock<IGenericRepository<OrderDetail>> _orderDetailsRepoMock = new();
    private readonly Mock<IGenericRepository<PaymentMethod>> _paymentMethodsRepoMock = new();
    private readonly Mock<IGenericRepository<Comment>> _commentsRepoMock = new();
    private readonly GameStoreDbContext _context;
    private readonly UnitOfWork _unitOfWork;

    public UnitOfWorkTests()
    {
        _context = DatabaseService.CreateSqLiteContext();
        _unitOfWork = new UnitOfWork(
            _context,
            _gameRepoMock.Object,
            _genreRepoMock.Object,
            _platformRepoMock.Object,
            _publisherRepoMock.Object,
            _gamesGenresRepoMock.Object,
            _gamesPlatformsRepoMock.Object,
            _ordersRepoMock.Object,
            _orderDetailsRepoMock.Object,
            _paymentMethodsRepoMock.Object,
            _commentsRepoMock.Object);
    }

    [Fact]
    public async Task SaveAsync_ShouldSaveChanges()
    {
        // Arrange
        var game = new Game
        {
            Name = "Game 1",
            Alias = "Game-1",
            Description = "test description",
        };
        await _context.Set<Game>().AddAsync(game);

        // Act
        await _unitOfWork.SaveAsync();

        // Assert
        var savedGame = await _context.Set<Game>().FindAsync(game.Id);
        Assert.NotNull(savedGame);
    }

    [Fact]
    public void Dispose_ShouldPreventFurtherOperationsOnDbContext()
    {
        // Arrange
        _unitOfWork.Dispose();

        // Act & Assert
        // After disposal, DbContext should throw an exception on usage
        Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.Games.AddAsync(new Game()));
    }

    public void Dispose()
    {
        _unitOfWork.Dispose();
        GC.SuppressFinalize(this);
    }
}