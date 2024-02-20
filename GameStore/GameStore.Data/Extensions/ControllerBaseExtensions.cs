using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Data.Extensions;

[ExcludeFromCodeCoverage]
public static class ControllerBaseExtensions
{
    public static string GetCurrentCultureName(this ControllerBase controller)
    {
        return controller.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
    }

    public static string GetAuthorizedUserId(this ControllerBase controller)
    {
        string? userId = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userId ?? throw new LoginException("User is not authorized");
    }
}