using System.Security.Claims;
using GameStore.API.Authorization.Requirements;
using GameStore.Application.Interfaces;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.API.Authorization.Handlers;

public class UpdateGameAuthorizationHandler : AuthorizationHandler<CanUpdateGameRequirement, GameUpdateDto>
{
    private readonly IGameFacadeService _gameFacadeService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGameAuthorizationHandler(IGameFacadeService gameFacadeService, IUnitOfWork unitOfWork)
    {
        _gameFacadeService = gameFacadeService;
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanUpdateGameRequirement requirement, GameUpdateDto resource)
    {
        if (context.User.HasPermission(PermissionOptions.GameFull))
        {
            context.Succeed(requirement);
            return;
        }

        var game = await _gameFacadeService.GetGameByIdAsync(resource.Game.Id, resource.Culture);
        if ((game.Deleted && context.User.HasPermission(PermissionOptions.GameUpdateDeleted)) ||
            (!game.Deleted && context.User.HasPermission(PermissionOptions.GameUpdate)))
        {
            context.Succeed(requirement);
            return;
        }

        string? userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (context.User.IsInRole(Roles.Publisher) &&
            context.User.HasPermission(PermissionOptions.GameUpdateOwn) &&
            userId is not null)
        {
            try
            {
                var publisher = await _unitOfWork.Publishers.GetOneAsync(p => p.AccountId.Equals(userId));
                if (game.PublisherId == publisher.Id.ToString())
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            catch (EntityNotFoundException)
            {
            }
        }

        context.Fail();
    }
}