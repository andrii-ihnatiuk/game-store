namespace GameStore.Shared.Constants;

public static class PermissionOptions
{
    public const string GameFull = "game:full";
    public const string GameViewDeleted = "game:view-deleted";
    public const string GameCreate = "game:create";
    public const string GameUpdate = "game:update";
    public const string GameUpdateDeleted = "game:update-deleted";
    public const string GameUpdateOwn = "game:update-own";
    public const string GameDelete = "game:delete";

    public const string GenreFull = "genre:full";
    public const string GenreCreate = "genre:create";
    public const string GenreUpdate = "genre:update";
    public const string GenreDelete = "genre:delete";

    public const string PublisherFull = "publisher:full";
    public const string PublisherCreate = "publisher:create";
    public const string PublisherUpdate = "publisher:update";
    public const string PublisherUpdateSelf = "publisher:update-self";
    public const string PublisherDelete = "publisher:delete";

    public const string PlatformFull = "platform:full";
    public const string PlatformCreate = "platform:create";
    public const string PlatformUpdate = "platform:update";
    public const string PlatformDelete = "platform:delete";

    public const string UserFull = "user:full";
    public const string UserView = "user:view";
    public const string UserCreate = "user:create";
    public const string UserUpdate = "user:update";
    public const string UserDelete = "user:delete";

    public const string RoleFull = "role:full";
    public const string RoleView = "role:view";
    public const string RoleCreate = "role:create";
    public const string RoleUpdate = "role:update";
    public const string RoleDelete = "role:delete";

    public const string OrderFull = "order:full";
    public const string OrderViewActive = "order:view-active";
    public const string OrderViewHistory = "order:view-history";
    public const string OrderUpdate = "order:update";

    public const string CommentFull = "comment:full";
    public const string CommentCreateOnDeleted = "comment:create-on-deleted";
    public const string CommentUpdate = "comment:update";
    public const string CommentUpdateOnDeleted = "comment:update-on-deleted";
    public const string CommentDelete = "comment:delete";
    public const string CommentBan = "comment:ban";

    public static readonly string[] AllOptions =
    {
        GameFull, GameViewDeleted, GameCreate, GameUpdate, GameUpdateDeleted, GameUpdateOwn, GameDelete,
        GenreFull, GenreCreate, GenreUpdate, GenreDelete,
        PublisherFull, PublisherCreate, PublisherUpdate, PublisherUpdateSelf, PublisherDelete,
        PlatformFull, PlatformCreate, PlatformUpdate, PlatformDelete,
        UserFull, UserView, UserCreate, UserUpdate, UserDelete,
        RoleFull, RoleView, RoleCreate, RoleUpdate, RoleDelete,
        OrderFull, OrderViewActive, OrderViewHistory, OrderUpdate,
        CommentFull, CommentCreateOnDeleted, CommentUpdate, CommentUpdateOnDeleted, CommentDelete, CommentBan,
    };
}