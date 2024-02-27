using System.Diagnostics.CodeAnalysis;
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
}