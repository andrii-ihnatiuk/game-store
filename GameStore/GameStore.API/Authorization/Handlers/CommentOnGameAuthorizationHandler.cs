using GameStore.API.Authorization.Requirements;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.API.Authorization.Handlers;

public class CommentOnGameAuthorizationHandler : AuthorizationHandler<CanCommentOnGameRequirement, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentOnGameAuthorizationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanCommentOnGameRequirement requirement, string resource)
    {
        if (context.User.HasPermission(PermissionOptions.CommentCreateOnDeleted) ||
            context.User.HasPermission(PermissionOptions.CommentFull))
        {
            context.Succeed(requirement);
            return;
        }

        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == resource);
        if (!game.Deleted)
        {
            context.Succeed(requirement);
        }
    }
}