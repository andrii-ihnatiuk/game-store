using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration;

[ExcludeFromCodeCoverage]
internal class GamePlatformJoinConfiguration : IEntityTypeConfiguration<GamePlatform>
{
    public void Configure(EntityTypeBuilder<GamePlatform> builder)
    {
        builder.HasKey(j => new { j.GameId, j.PlatformId });

        builder
            .HasOne(j => j.Game)
            .WithMany(g => g.GamePlatforms)
            .HasForeignKey(j => j.GameId);

        builder
            .HasOne(j => j.Platform)
            .WithMany(p => p.PlatformGames)
            .HasForeignKey(j => j.PlatformId);
    }
}