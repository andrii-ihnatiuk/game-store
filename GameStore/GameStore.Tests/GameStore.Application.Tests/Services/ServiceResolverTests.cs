using GameStore.Application.Interfaces;
using GameStore.Application.Services;
using GameStore.Shared.Constants;
using GameStore.Shared.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Northwind.Services.Util;

namespace GameStore.Tests.GameStore.Application.Tests.Services;

public class ServiceResolverTests
{
    private readonly ServiceResolver _serviceResolver;
    private readonly Mock<IServiceProviderWrapper> _mockServiceProvider;
    private readonly Mock<IGameService> _sqlServiceMock = new();
    private readonly Mock<IGameService> _mongoServiceMock = new();
    private readonly IServiceCollection _serviceCollection = new ServiceCollection();

    public ServiceResolverTests()
    {
        _mockServiceProvider = new Mock<IServiceProviderWrapper>();
        _serviceResolver = new ServiceResolver(_mockServiceProvider.Object);

        _sqlServiceMock.Setup(s => s.EntityStorage).Returns(EntityStorage.SqlServer);
        _mongoServiceMock.Setup(s => s.EntityStorage).Returns(EntityStorage.MongoDb);

        _serviceCollection.AddSingleton(_sqlServiceMock.Object);
        _serviceCollection.AddSingleton(_mongoServiceMock.Object);
    }

    [Fact]
    public void ResolveForEntityId_WhenPassedGuid_ReturnsSqlServerService()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        _mockServiceProvider.Setup(p => p.GetServices<IGameService>())
                            .Returns(_serviceCollection.BuildServiceProvider().GetServices<IGameService>());

        // Act
        var resolved = _serviceResolver.ResolveForEntityId<IGameService>(id);

        // Assert
        Assert.Equal(_sqlServiceMock.Object, resolved);
    }

    [Fact]
    public void ResolveForEntityId_WhenPassedNonGuidString_ReturnsMongoService()
    {
        // Arrange
        const string id = "not-a-guid";
        _mockServiceProvider.Setup(p => p.GetServices<IGameService>())
                            .Returns(_serviceCollection.BuildServiceProvider().GetServices<IGameService>());

        // Act
        var resolved = _serviceResolver.ResolveForEntityId<IGameService>(id);

        // Assert
        Assert.Equal(_mongoServiceMock.Object, resolved);
    }

    [Fact]
    public void ResolveForEntityAlias_WhenAliasDoesNotEndWithSuffix_ReturnsSqlServerService()
    {
        // Arrange
        const string alias = "SomeAlias";
        _mockServiceProvider.Setup(p => p.GetServices<IGameService>())
            .Returns(_serviceCollection.BuildServiceProvider().GetServices<IGameService>());

        // Act
        var resolved = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);

        // Assert
        Assert.Equal(_sqlServiceMock.Object, resolved);
    }

    [Fact]
    public void ResolveForEntityAlias_WhenAliasEndsWithSuffix_ReturnsMongoService()
    {
        // Arrange
        const string alias = $"some-alias{EntityAliasUtil.AliasSuffix}";
        _mockServiceProvider.Setup(p => p.GetServices<IGameService>())
            .Returns(_serviceCollection.BuildServiceProvider().GetServices<IGameService>());

        // Act
        var resolved = _serviceResolver.ResolveForEntityAlias<IGameService>(alias);

        // Assert
        Assert.Equal(_mongoServiceMock.Object, resolved);
    }

    [Fact]
    public void ResolveAll_ReturnAllResolvedServices()
    {
        // Arrange
        _mockServiceProvider.Setup(p => p.GetServices<IGameService>())
            .Returns(_serviceCollection.BuildServiceProvider().GetServices<IGameService>());

        // Act
        var resolved = _serviceResolver.ResolveAll<IGameService>().ToList();

        // Assert
        Assert.Contains(_sqlServiceMock.Object, resolved);
        Assert.Contains(_mongoServiceMock.Object, resolved);
    }
}