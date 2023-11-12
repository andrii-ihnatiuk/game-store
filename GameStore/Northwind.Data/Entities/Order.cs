using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Northwind.Data.Entities;

public class Order
{
    public ObjectId Id { get; set; }

    [BsonElement("CustomerID")]
    public string CustomerId { get; set; }
}