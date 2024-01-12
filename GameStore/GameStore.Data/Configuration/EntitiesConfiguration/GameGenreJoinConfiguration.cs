using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
internal class GameGenreJoinConfiguration : IEntityTypeConfiguration<GameGenre>
{
    public void Configure(EntityTypeBuilder<GameGenre> builder)
    {
        builder.HasKey(j => new { j.GameId, j.GenreId });

        builder
            .HasOne(j => j.Game)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(j => j.GameId);

        builder
            .HasOne(j => j.Genre)
            .WithMany(gnr => gnr.GenreGames)
            .HasForeignKey(j => j.GenreId);
    }
}