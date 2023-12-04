namespace GameStore.Data.Interfaces;

public interface IMigrationTrackable
{
    public string? LegacyId { get; set; }
}