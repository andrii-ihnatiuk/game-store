using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;

namespace Northwind.Data.Entities;

[BsonCollection("categories")]
public class Category : BaseEntity
{
    [BsonElement("CategoryID")]
    public long CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

    public string Picture { get; set; }

    [BsonElement("ParentID")]
    public string? ParentId { get; set; }
}