using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class OrderDetailEntityConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(od => od.Id);

        builder.Property(od => od.Price)
            .HasColumnType("money");

        builder.HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(od => od.ProductId);

        builder.HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId);
    }
}