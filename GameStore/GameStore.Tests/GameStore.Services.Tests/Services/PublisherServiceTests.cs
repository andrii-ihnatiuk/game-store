using System.Linq.Expressions;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Interfaces;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Publisher;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class PublisherServiceTests
{
    private const string CompanyName = "test";
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly PublisherService _service;

    public PublisherServiceTests()
    {
        _service = new PublisherService(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetPublisherByNameAsync_ReturnsPublisher()
    {
        // Arrange
        var publisher = new Publisher { CompanyName = CompanyName };
        _unitOfWork.Setup(uow => uow.Publishers.GetOneAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(publisher)
            .Verifiable();

        _mapper.Setup(m => m.Map<PublisherFullDto>(publisher))
            .Returns(new PublisherFullDto());

        // Act
        var result = await _service.GetPublisherByNameAsync(CompanyName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PublisherFullDto>(result);
        _unitOfWork.Verify();
    }

    [Fact]
    public async Task GetAllPublishersAsync_ReturnsPublishers()
    {
        // Arrange
        var publishers = new List<Publisher> { new(), new() };
        _unitOfWork.Setup(uow => uow.Publishers.GetAsync(
                It.IsAny<Expression<Func<Publisher, bool>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IOrderedQueryable<Publisher>>>(),
                It.IsAny<Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(publishers)
            .Verifiable();

        _mapper.Setup(m => m.Map<IList<PublisherBriefDto>>(publishers))
            .Returns(new List<PublisherBriefDto> { new(), new() });

        // Act
        var result = await _service.GetAllPublishersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IList<PublisherBriefDto>>(result);
        _unitOfWork.Verify();
    }

    [Fact]
    public async Task GetGamesByPublisherNameAsync_ReturnsGames()
    {
        // Arrange
        var games = new List<Game> { new(), new() };
        _unitOfWork.Setup(uow => uow.Games.GetAsync(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Func<IQueryable<Game>, IOrderedQueryable<Game>>>(),
                It.IsAny<Func<IQueryable<Game>, IIncludableQueryable<Game, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(games)
            .Verifiable();

        _mapper.Setup(m => m.Map<IList<GameBriefDto>>(games))
            .Returns(games.Select(game => new GameBriefDto()).ToList());

        // Act
        var result = await _service.GetGamesByPublisherNameAsync(CompanyName);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IList<GameBriefDto>>(result);
        _unitOfWork.Verify();
    }

    [Fact]
    public async Task AddPublisherAsync_DoesNotThrow()
    {
        // Arrange
        var dto = new PublisherCreateDto { Publisher = new PublisherCreateInnerDto { CompanyName = CompanyName } };

        _unitOfWork.Setup(uow => uow.Publishers.ExistsAsync(p => p.CompanyName == CompanyName))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Publishers.AddAsync(It.IsAny<Publisher>()))
            .Returns(Task.CompletedTask);

        _unitOfWork.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

        _mapper.Setup(m => m.Map<Publisher>(dto)).Returns(new Publisher());
        _mapper.Setup(m => m.Map<PublisherBriefDto>(It.IsAny<Publisher>())).Returns(new PublisherBriefDto());

        // Act
        await _service.AddPublisherAsync(dto);

        // Assert
        _unitOfWork.Verify(uow => uow.Publishers.AddAsync(It.IsAny<Publisher>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPublisherAsync_WhenCompanyNameExists_ThrowsException()
    {
        // Arrange
        var dto = new PublisherCreateDto { Publisher = new PublisherCreateInnerDto { CompanyName = CompanyName } };

        _unitOfWork.Setup(uow => uow.Publishers.ExistsAsync(p => p.CompanyName == CompanyName))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.AddPublisherAsync(dto));
        _unitOfWork.Verify(uow => uow.Publishers.AddAsync(It.IsAny<Publisher>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdatePublisherAsync_DoesNotThrow()
    {
        // Arrange
        var dto = new PublisherUpdateDto { Publisher = new PublisherUpdateInnerDto { Id = Guid.Empty, CompanyName = CompanyName } };
        var existingPublisher = new Publisher { CompanyName = "another-company" };

        _unitOfWork.Setup(uow => uow.Publishers.GetByIdAsync(dto.Publisher.Id)).ReturnsAsync(existingPublisher);

        _unitOfWork.Setup(uow => uow.Publishers.ExistsAsync(p => p.CompanyName == existingPublisher.CompanyName))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

        // Act
        await _service.UpdatePublisherAsync(dto);

        // Assert
        _unitOfWork.Verify(uow => uow.Publishers.GetByIdAsync(dto.Publisher.Id), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisherAsync_WhenCompanyNameExists_ThrowsException()
    {
        // Arrange
        var dto = new PublisherUpdateDto { Publisher = new PublisherUpdateInnerDto { Id = Guid.Empty, CompanyName = "different-company" } };
        var existingPublisher = new Publisher { Id = dto.Publisher.Id, CompanyName = CompanyName };

        _unitOfWork.Setup(uow => uow.Publishers.GetByIdAsync(dto.Publisher.Id)).ReturnsAsync(existingPublisher);

        _unitOfWork.Setup(uow => uow.Publishers.ExistsAsync(p => p.CompanyName == "different-company"))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.UpdatePublisherAsync(dto));
        _unitOfWork.Verify(uow => uow.Publishers.GetByIdAsync(dto.Publisher.Id), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeletePublisherAsync_DoesNotThrow()
    {
        // Arrange
        var publisherId = Guid.Empty;
        _unitOfWork.Setup(uow => uow.Publishers.DeleteAsync(publisherId))
            .Returns(Task.CompletedTask);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        // Act
        await _service.DeletePublisherAsync(publisherId);

        // Assert
        _unitOfWork.Verify(uow => uow.Publishers.DeleteAsync(publisherId), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}