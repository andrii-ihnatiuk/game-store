using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration;

internal class GameEntityConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder
            .HasIndex(e => e.Alias).IsUnique(true);

        builder
            .HasKey(e => e.Id);

        builder
            .HasOne(g => g.Genre)
            .WithMany(gnr => gnr.Games)
            .HasForeignKey(g => g.GenreId);
    }
}