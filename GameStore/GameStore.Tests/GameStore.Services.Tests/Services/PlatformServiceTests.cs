using System.Linq.Expressions;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Exceptions;
using GameStore.Data.Repositories;
using GameStore.Services.Services;
using GameStore.Shared.DTOs.Platform;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace GameStore.Tests.GameStore.Services.Tests.Services;

public class PlatformServiceTests
{
    private const string PlatformType = "test";
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly PlatformService _service;

    public PlatformServiceTests()
    {
        _service = new PlatformService(_unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetPlatformByIdAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        var id = Guid.Empty;
        var platform = new Platform { Id = id };
        _unitOfWork.Setup(uow => uow.Platforms.GetOneAsync(
                It.IsAny<Expression<Func<Platform, bool>>>(),
                It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(platform);

        _mapper.Setup(m => m.Map<PlatformFullDto>(platform))
            .Returns(new PlatformFullDto());

        // Act
        await _service.GetPlatformByIdAsync(id);

        // Assert
        _unitOfWork.Verify(
            uow => uow.Platforms.GetOneAsync(
                p => p.Id == id,
                It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>(),
                It.IsAny<bool>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllPlatformsAsync_ReturnsPlatforms()
    {
        // Arrange
        var platformsData = new List<Platform> { new(), new() };
        _unitOfWork.Setup(uow => uow.Platforms.GetAsync(
                It.IsAny<Expression<Func<Platform, bool>>>(),
                It.IsAny<Func<IQueryable<Platform>, IOrderedQueryable<Platform>>>(),
                It.IsAny<Func<IQueryable<Platform>, IIncludableQueryable<Platform, object>>>(),
                It.IsAny<bool>()))
            .ReturnsAsync(platformsData);

        _mapper.Setup(m => m.Map<IList<PlatformBriefDto>>(platformsData))
            .Returns(new List<PlatformBriefDto> { new(), new() });

        // Act
        var platforms = await _service.GetAllPlatformsAsync();

        // Assert
        Assert.Equal(platformsData.Count, platforms.Count);
    }

    [Fact]
    public async Task AddPlatformAsync_AllOk_CallsRepository()
    {
        // Arrange
        var platformCreateDto = new PlatformCreateDto();
        var newPlatform = new Platform { Type = PlatformType };

        _mapper.Setup(m => m.Map<Platform>(platformCreateDto))
            .Returns(newPlatform);

        _unitOfWork.Setup(uow => uow.Platforms.ExistsAsync(p => p.Type == PlatformType))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.Platforms.AddAsync(newPlatform))
            .Returns(Task.CompletedTask);

        _unitOfWork.Setup(x => x.SaveAsync())
            .ReturnsAsync(1);

        _mapper.Setup(m => m.Map<PlatformFullDto>(newPlatform))
            .Returns(new PlatformFullDto());

        // Act
        await _service.AddPlatformAsync(platformCreateDto);

        // Assert
        _unitOfWork.Verify(uow => uow.Platforms.AddAsync(newPlatform), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddPlatformAsync_WhenDuplicateType_ThrowsEntityAlreadyExistsException()
    {
        // Arrange
        var platformCreateDto = new PlatformCreateDto();
        var newPlatform = new Platform { Type = PlatformType };

        _mapper.Setup(m => m.Map<Platform>(platformCreateDto))
            .Returns(newPlatform);

        _unitOfWork.Setup(x => x.Platforms.ExistsAsync(p => p.Type == PlatformType))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.AddPlatformAsync(platformCreateDto));
    }

    [Fact]
    public async Task UpdatePlatformAsync_AllOk_CallsRepository()
    {
        // Arrange
        var platformUpdateDto = new PlatformUpdateDto() { Id = Guid.Empty, Type = PlatformType };
        var existingPlatform = new Platform() { Type = PlatformType };

        _unitOfWork.Setup(uow => uow.Platforms.GetByIdAsync(platformUpdateDto.Id))
            .ReturnsAsync(existingPlatform);

        _unitOfWork.Setup(uow => uow.Platforms.ExistsAsync(p => p.Type == PlatformType))
            .ReturnsAsync(false);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        // Act
        await _service.UpdatePlatformAsync(platformUpdateDto);

        // Assert
        _unitOfWork.Verify(uow => uow.Platforms.GetByIdAsync(platformUpdateDto.Id), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePlatformAsync_WhenDuplicateType_ThrowsException()
    {
        // Arrange
        const string updatedType = "updated-but-already-exists";
        var platformUpdateDto = new PlatformUpdateDto() { Type = updatedType };
        var existingPlatform = new Platform() { Type = PlatformType };

        _unitOfWork.Setup(uow => uow.Platforms.GetByIdAsync(platformUpdateDto.Id))
            .ReturnsAsync(existingPlatform);

        _unitOfWork.Setup(uow => uow.Platforms.ExistsAsync(p => p.Type == updatedType))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<EntityAlreadyExistsException>(() => _service.UpdatePlatformAsync(platformUpdateDto));
    }

    [Fact]
    public async Task DeletePlatformAsync_CallsRepository_WithValidArguments()
    {
        // Arrange
        var platformId = Guid.Empty;
        _unitOfWork.Setup(uow => uow.Platforms.DeleteAsync(platformId))
            .Returns(Task.CompletedTask);

        _unitOfWork.Setup(uow => uow.SaveAsync())
            .ReturnsAsync(1);

        // Act
        await _service.DeletePlatformAsync(platformId);

        // Assert
        _unitOfWork.Verify(uow => uow.Platforms.DeleteAsync(platformId), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
    }
}