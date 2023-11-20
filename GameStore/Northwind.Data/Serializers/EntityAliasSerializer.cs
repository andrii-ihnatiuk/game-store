using System.Diagnostics.CodeAnalysis;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Northwind.Data.Serializers;

[ExcludeFromCodeCoverage]
public class EntityAliasSerializer : SerializerBase<string>
{
    public const string AliasSuffix = "_northwind";

    public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var type = context.Reader.GetCurrentBsonType();
        return type switch
        {
            BsonType.String => HttpUtility.UrlEncode(string.Concat(context.Reader.ReadString(), AliasSuffix)),
            _ => throw new NotSupportedException($"Deserialization from {type} to string is not supported."),
        };
    }
}