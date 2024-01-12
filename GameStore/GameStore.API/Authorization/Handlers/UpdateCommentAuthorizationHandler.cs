using GameStore.API.Authorization.Requirements;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Authorization.Handlers;

public class UpdateCommentAuthorizationHandler : AuthorizationHandler<CanUpdateCommentRequirement, CommentUpdateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommentAuthorizationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanUpdateCommentRequirement requirement, CommentUpdateDto resource)
    {
        if (context.User.HasPermission(PermissionOptions.CommentFull))
        {
            context.Succeed(requirement);
            return;
        }

        var commentId = resource.Comment.Id;
        var comment = await _unitOfWork.Comments.GetOneAsync(
            predicate: c => c.Id.Equals(commentId),
            include: q => q.Include(c => c.Game));

        if ((comment.Game.Deleted && context.User.HasPermission(PermissionOptions.CommentUpdateOnDeleted)) ||
            (!comment.Game.Deleted && context.User.HasPermission(PermissionOptions.CommentUpdate)))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}