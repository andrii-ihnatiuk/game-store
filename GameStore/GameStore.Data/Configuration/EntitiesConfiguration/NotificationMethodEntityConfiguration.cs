using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class NotificationMethodEntityConfiguration : IEntityTypeConfiguration<NotificationMethod>
{
    public void Configure(EntityTypeBuilder<NotificationMethod> builder)
    {
        builder.HasIndex(e => e.NormalizedName)
            .IsUnique();
    }
}