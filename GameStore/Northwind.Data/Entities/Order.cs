using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;

namespace Northwind.Data.Entities;

[BsonCollection("orders")]
public class Order : BaseEntity
{
    [BsonElement("OrderID")]
    public long OrderId { get; set; }

    [BsonElement("CustomerID")]
    public string CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    [BsonIgnore]
    public IList<OrderDetail> OrderDetails { get; set; }
}