using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;

namespace Northwind.Data.Entities;

[BsonCollection("entity-log")]
public class EntityLog : BaseEntity
{
    public DateTime Date { get; set; }

    public string Action { get; set; }

    public string EntityType { get; set; }

    [BsonIgnoreIfNull]
    public BsonDocument? Entity { get; set; }

    [BsonIgnoreIfNull]
    public BsonDocument? UpdEntity { get; set; }
}