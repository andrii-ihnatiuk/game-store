namespace GameStore.Data.Entities;

public class NotificationMethod
{
    private string _name;

    public Guid Id { get; set; }

    public string Name
    {
        get => _name;
        set
        {
            _name = value.Trim();
            NormalizedName = _name.ToUpperInvariant();
        }
    }

    public string NormalizedName { get; set; }

    public IList<UserNotificationMethod> NotificationMethodUsers { get; set; }
}