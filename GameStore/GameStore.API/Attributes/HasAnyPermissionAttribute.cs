using GameStore.Data.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameStore.API.Attributes;

public class HasAnyPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string[] _permissions;

    public HasAnyPermissionAttribute(params string[] permissions)
    {
        _permissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool hasClaim = _permissions.Any(p => context.HttpContext.User.HasPermission(p));
        if (!hasClaim)
        {
            context.Result = new ForbidResult();
        }
    }
}