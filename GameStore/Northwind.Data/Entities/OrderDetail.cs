using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;

namespace Northwind.Data.Entities;

[BsonCollection("order-details")]
public class OrderDetail : BaseEntity
{
    [BsonElement("OrderID")]
    public long OrderId { get; set; }

    [BsonElement("ProductID")]
    public long ProductId { get; set; }

    [BsonElement("UnitPrice")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public float Discount { get; set; }
}