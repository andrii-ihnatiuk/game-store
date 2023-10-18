using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration;

[ExcludeFromCodeCoverage]
internal class GameEntityConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder.HasIndex(g => g.Alias).IsUnique(true);

        builder
            .Property(g => g.Price)
            .HasColumnType("money");

        builder
            .Property(g => g.Discontinued)
            .HasColumnType("bit");
    }
}