using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;

namespace Northwind.Data.Entities;

[BsonCollection("orders")]
public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("CustomerID")]
    public string CustomerId { get; set; }

    public string OrderDate { get; set; }
}