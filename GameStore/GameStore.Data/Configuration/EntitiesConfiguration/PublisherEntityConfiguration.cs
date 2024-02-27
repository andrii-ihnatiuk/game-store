using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class PublisherEntityConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.CompanyName).IsUnique(true);

        builder.HasMany(p => p.Games)
            .WithOne(g => g.Publisher)
            .HasForeignKey(g => g.PublisherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Publisher>(p => p.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(p => p.CompanyName)
            .HasColumnType("nvarchar(40)");

        builder.Property(p => p.Description)
            .HasColumnType("ntext");

        builder.Property(p => p.HomePage)
            .HasColumnType("ntext");

        builder.HasMany(p => p.Translations)
            .WithOne(t => t.Core)
            .HasForeignKey(t => t.CoreId);
    }
}