using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;

namespace Northwind.Data.Entities;

[BsonCollection("products")]
public class Product : BaseEntity
{
    [BsonElement("ProductID")]
    public long ProductId { get; set; }

    public string ProductName { get; set; }

    public string Alias { get; set; }

    [BsonElement("SupplierID")]
    public long SupplierId { get; set; }

    [BsonElement("CategoryID")]
    public long CategoryId { get; set; }

    public string QuantityPerUnit { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal UnitPrice { get; set; }

    public short UnitsInStock { get; set; }

    public uint UnitsOnOrder { get; set; }

    public uint ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    public bool Deleted { get; set; }
}