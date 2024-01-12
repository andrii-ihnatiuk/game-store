using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

[ExcludeFromCodeCoverage]
public sealed class GameStoreDbContext :
    IdentityDbContext<
        ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
        ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
        : base(options)
    {
    }

    public bool LogChanges { get; set; } = true;

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        builder.SeedData();
    }

    private void SetTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e is { State: EntityState.Added, Entity: ICreationTrackable });

        foreach (var entry in entries)
        {
            ((ICreationTrackable)entry.Entity).CreationDate = DateTime.UtcNow;
        }
    }
}
