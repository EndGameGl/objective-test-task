using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objective.PriceTracker.Api.Models.Database;

namespace Objective.PriceTracker.Api.Services.Database.EntityTypeConfigurations;

public class DbSubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<DbSubscription>
{
    public void Configure(EntityTypeBuilder<DbSubscription> builder)
    {
        builder.ToTable("subscriptions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder
            .HasOne(x => x.Subscriber)
            .WithMany()
            .HasForeignKey(x => x.SubscriberId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.ApartmentTracker)
            .WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.ApartmentTrackerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(x => new { x.SubscriberId, x.ApartmentTrackerId }).IsUnique();
    }
}
