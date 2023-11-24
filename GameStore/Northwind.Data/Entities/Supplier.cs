using MongoDB.Bson.Serialization.Attributes;
using Northwind.Data.Attributes;

namespace Northwind.Data.Entities;

[BsonCollection("suppliers")]
public class Supplier : BaseEntity
{
    [BsonElement("SupplierID")]
    public long SupplierId { get; set; }

    public string CompanyName { get; set; }

    public string ContactTitle { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string? Region { get; set; }

    public string PostalCode { get; set; }

    public string Country { get; set; }

    public string Phone { get; set; }

    public string? Fax { get; set; }

    public string? HomePage { get; set; }
}