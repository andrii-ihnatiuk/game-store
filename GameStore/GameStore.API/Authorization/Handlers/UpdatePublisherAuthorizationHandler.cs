using System.Security.Claims;
using GameStore.API.Authorization.Requirements;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.API.Authorization.Handlers;

public class UpdatePublisherAuthorizationHandler : AuthorizationHandler<CanUpdatePublisherRequirement, PublisherUpdateDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePublisherAuthorizationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanUpdatePublisherRequirement requirement,
        PublisherUpdateDto resource)
    {
        if (context.User.HasPermission(PermissionOptions.PublisherFull) ||
            context.User.HasPermission(PermissionOptions.PublisherUpdate))
        {
            context.Succeed(requirement);
            return;
        }

        string? userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId is null || !context.User.HasPermission(PermissionOptions.PublisherUpdateSelf))
        {
            context.Fail();
            return;
        }

        if (resource.Publisher.Id.IsGuidFormat())
        {
            var publisher = await _unitOfWork.Publishers.GetOneAsync(p => p.Id.Equals(Guid.Parse(resource.Publisher.Id)));
            if (publisher.AccountId == userId)
            {
                context.Succeed(requirement);
                return;
            }
        }

        context.Fail();
    }
}