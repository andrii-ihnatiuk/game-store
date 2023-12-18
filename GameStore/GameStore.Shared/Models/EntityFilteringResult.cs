namespace GameStore.Shared.Models;

public struct EntityFilteringResult<T>
    where T : class
{
    public EntityFilteringResult(IList<T> records, int totalNoLimit)
    {
        Records = records;
        TotalNoLimit = totalNoLimit;
    }

    public IList<T> Records { get; set; }

    public int TotalNoLimit { get; set; }

    public IList<string> MongoBlacklist { get; set; } = new List<string>();
}