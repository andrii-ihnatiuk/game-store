using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class PaymentMethodEntityConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.Title).IsUnique(true);
    }
}