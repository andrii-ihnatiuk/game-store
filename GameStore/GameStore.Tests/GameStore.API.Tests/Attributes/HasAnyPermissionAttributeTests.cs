using System.Security.Claims;
using GameStore.API.Attributes;
using GameStore.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace GameStore.Tests.GameStore.API.Tests.Attributes;

public class HasAnyPermissionAttributeTests
{
    private AuthorizationFilterContext _authContext;

    [Fact]
    public void OnAuthorization_UserHasAtLeastOnePermission_NoAction()
    {
        // Arrange
        string[] permissions = { PermissionOptions.GameUpdate, PermissionOptions.GameFull };
        var attribute = new HasAnyPermissionAttribute(permissions);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(CustomClaimTypes.Permission, PermissionOptions.GameUpdate),
        }));
        SetupAuthorizationContext(principal);

        // Act
        attribute.OnAuthorization(_authContext);

        // Assert
        Assert.Null(_authContext.Result);
    }

    [Fact]
    public void OnAuthorization_UserDoesNotHavePermission_SetForbidResult()
    {
        // Arrange
        string[] permissions = { PermissionOptions.GameUpdate, PermissionOptions.GameFull };
        var attribute = new HasAnyPermissionAttribute(permissions);
        SetupAuthorizationContext(new ClaimsPrincipal());

        // Act
        attribute.OnAuthorization(_authContext);

        // Assert
        Assert.IsType<ForbidResult>(_authContext.Result);
    }

    private void SetupAuthorizationContext(ClaimsPrincipal principal)
    {
        var actionContext = new ActionContext()
        {
            HttpContext = new DefaultHttpContext { User = principal },
            RouteData = new RouteData(),
            ActionDescriptor = new ActionDescriptor(),
        };
        _authContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }
}