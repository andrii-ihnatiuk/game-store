namespace GameStore.Data.Entities;

public class AppImage
{
    public Guid Id { get; set; }

    public string Large { get; set; }

    public string? Small { get; set; }

    public bool IsCover { get; set; }

    public ushort Order { get; set; }

    public Guid? GameId { get; set; }

    public Game? Game { get; set; }

    public void ResetOwner()
    {
        GameId = null;
        IsCover = false;
    }
}