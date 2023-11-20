#pragma warning disable IDE0055

// using AutoMapper;
// using GameStore.Application.Services;
// using GameStore.Data.Interfaces;
// using GameStore.Services;
// using GameStore.Shared.Interfaces.Services;
// using Moq;
// using Northwind.Data.Interfaces;
// using Northwind.Services;
//
// namespace GameStore.Tests.GameStore.Application.Tests.Services;
//
// public class OrderServiceProviderTests
// {
//     private readonly Mock<IUnitOfWork> _coreUnitOfWorkMock = new();
//     private readonly Mock<IMongoUnitOfWork> _mongoUnitOfWorkMock = new();
//     private readonly Mock<IMapper> _mapperMock = new();
//     private readonly OrderServiceProvider _orderServiceProvider;
//
//     public OrderServiceProviderTests()
//     {
//         CoreOrderService coreOrderService = new(_coreUnitOfWorkMock.Object, _mapperMock.Object);
//         MongoOrderService mongoOrderService = new(_mongoUnitOfWorkMock.Object, _mapperMock.Object);
//         var services = new List<IOrderService> { coreOrderService, mongoOrderService };
//         _orderServiceProvider = new OrderServiceProvider(services);
//     }
//
//     [Fact]
//     public void GetByIdString_WhenCalledWithGuidId_ReturnsCoreService()
//     {
//         // Arrange && Act
//         var service = _orderServiceProvider.GetByIdString(Guid.Empty.ToString());
//
//         // Assert
//         Assert.IsType<CoreOrderService>(service);
//     }
//
//     [Fact]
//     public void GetByIdString_WhenCalledWithStringId_ReturnsMongoService()
//     {
//         // Arrange && Act
//         var service = _orderServiceProvider.GetByIdString("string-id");
//
//         // Assert
//         Assert.IsType<MongoOrderService>(service);
//     }
//
//     [Fact]
//     public void GetAll_WhenCalled_ReturnsAllServices()
//     {
//         // Arrange && Act
//         var services = _orderServiceProvider.GetAll();
//
//         // Assert
//         Assert.IsAssignableFrom<IEnumerable<IOrderService>>(services);
//     }
// }
#pragma warning restore IDE0055