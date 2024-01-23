using System.Diagnostics.CodeAnalysis;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Data.Configuration.EntitiesConfiguration;

[ExcludeFromCodeCoverage]
public class UserNotificationMethodJoinConfiguration : IEntityTypeConfiguration<UserNotificationMethod>
{
    public void Configure(EntityTypeBuilder<UserNotificationMethod> builder)
    {
        builder.HasKey(e => new { e.UserId, e.NotificationMethodId });

        builder.HasOne(e => e.User)
            .WithMany(u => u.NotificationMethods)
            .HasForeignKey(e => e.UserId);

        builder.HasOne(e => e.NotificationMethod)
            .WithMany(x => x.NotificationMethodUsers)
            .HasForeignKey(e => e.NotificationMethodId);
    }
}