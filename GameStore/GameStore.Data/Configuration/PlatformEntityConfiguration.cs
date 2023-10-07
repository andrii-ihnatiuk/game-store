using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration;

public class PlatformEntityConfiguration : IEntityTypeConfiguration<Platform>
{
    public void Configure(EntityTypeBuilder<Platform> builder)
    {
        builder.HasIndex(p => p.Type).IsUnique(true);

        builder
            .HasMany(p => p.Games)
            .WithOne(g => g.Platform)
            .HasForeignKey(g => g.PlatformId);
    }
}