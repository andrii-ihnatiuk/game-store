using AutoFixture;
using GameStore.Application.Interfaces.Util;
using GameStore.Application.Util;
using GameStore.Services.Interfaces.Authentication;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.User;
using Moq;

namespace GameStore.Tests.GameStore.Application.Tests.Util;

public class LoginServiceResolverTests
{
    private readonly LoginServiceResolver _resolver;
    private readonly Fixture _fixture = new();
    private readonly Mock<IServiceProviderWrapper> _serviceProviderMock = new();
    private readonly Mock<ILoginService> _internalLoginServiceMock = new();
    private readonly Mock<ILoginService> _externalLoginServiceMock = new();

    public LoginServiceResolverTests()
    {
        _internalLoginServiceMock.Setup(s => s.LoginMethod).Returns(LoginMethod.Internal);
        _externalLoginServiceMock.Setup(s => s.LoginMethod).Returns(LoginMethod.AuthApi);

        var services = new List<ILoginService>
        {
            _internalLoginServiceMock.Object,
            _externalLoginServiceMock.Object,
        };
        _serviceProviderMock.Setup(s => s.GetServices<ILoginService>())
            .Returns(services);

        _resolver = new LoginServiceResolver(_serviceProviderMock.Object);
    }

    [Theory]
    [InlineData(true, LoginMethod.Internal)]
    [InlineData(false, LoginMethod.AuthApi)]
    public void Resolve_GivenLoginModel_ReturnsCorrectService(bool isInternalAuth, LoginMethod loginMethod)
    {
        // Arrange
        var loginModel = _fixture.Build<UserLoginModel>().With(m => m.InternalAuth, isInternalAuth).Create();

        // Act
        var resolvedService = _resolver.Resolve(loginModel);

        // Assert
        Assert.Equal(loginMethod, resolvedService.LoginMethod);
    }
}