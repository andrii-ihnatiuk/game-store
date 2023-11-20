using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;
using Northwind.Data.Serializers;

namespace Northwind.Data.Entities;

[BsonCollection("products")]
public class Product : BaseEntity
{
    [BsonElement("ProductID")]
    public long ProductId { get; set; }

    public string ProductName { get; set; }

    [BsonSerializer(typeof(EntityAliasSerializer))]
    public string Alias { get; set; }

    [BsonElement("SupplierID")]
    public long SupplierId { get; set; }

    [BsonElement("CategoryID")]
    public long CategoryId { get; set; }

    public string QuantityPerUnit { get; set; }

    public decimal UnitPrice { get; set; }

    public short UnitsInStock { get; set; }

    public uint UnitsOnOrder { get; set; }

    public uint ReorderLevel { get; set; }

    public bool Discontinued { get; set; }
}