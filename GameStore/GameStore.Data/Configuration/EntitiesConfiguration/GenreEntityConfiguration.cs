﻿using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
internal class GenreEntityConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(gnr => gnr.Id);

        builder.HasIndex(gnr => gnr.Name).IsUnique(true);

        builder
            .HasMany(gnr => gnr.SubGenres)
            .WithOne()
            .HasForeignKey(gnr => gnr.ParentGenreId);

        builder.HasMany(g => g.Translations)
            .WithOne(t => t.Core)
            .HasForeignKey(t => t.CoreId);
    }
}