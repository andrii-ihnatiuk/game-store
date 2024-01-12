namespace GameStore.Shared.Constants;

public static class Roles
{
    public const string Admin = "Administrator";
    public const string Manager = "Manager";
    public const string Moderator = "Moderator";
    public const string Publisher = "Publisher";
    public const string User = "User";

    public static readonly string[] AllRoles =
    {
        Admin, Manager, Moderator, Publisher, User,
    };
}