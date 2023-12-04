namespace GameStore.Shared.Interfaces;

public interface ILegacyFilterable
{
    public string Id { get; set; }

    public string? LegacyId { get; set; }
}