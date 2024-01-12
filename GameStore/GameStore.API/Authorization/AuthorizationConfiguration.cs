using System.Diagnostics.CodeAnalysis;
using GameStore.API.Authorization.Handlers;
using GameStore.API.Authorization.Requirements;
using GameStore.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace GameStore.API.Authorization;

[ExcludeFromCodeCoverage]
public static class AuthorizationConfiguration
{
    public static void ConfigureAuthorization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.CanUpdateGame, policy =>
                policy.AddRequirements(new CanUpdateGameRequirement()));
            options.AddPolicy(PolicyNames.CanUpdatePublisher, policy =>
                policy.AddRequirements(new CanUpdatePublisherRequirement()));
            options.AddPolicy(PolicyNames.CanCommentOnGame, policy =>
                policy.AddRequirements(new CanCommentOnGameRequirement()));
            options.AddPolicy(PolicyNames.CanUpdateComment, policy =>
                policy.AddRequirements(new CanUpdateCommentRequirement()));
        });

        serviceCollection.AddScoped<IAuthorizationHandler, UpdateGameAuthorizationHandler>();
        serviceCollection.AddScoped<IAuthorizationHandler, UpdatePublisherAuthorizationHandler>();
        serviceCollection.AddScoped<IAuthorizationHandler, CommentOnGameAuthorizationHandler>();
        serviceCollection.AddScoped<IAuthorizationHandler, UpdateCommentAuthorizationHandler>();
    }
}