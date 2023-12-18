﻿using System.Diagnostics.CodeAnalysis;

namespace Northwind.Data.Attributes;

[ExcludeFromCodeCoverage]

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }

    public string CollectionName { get; }
}