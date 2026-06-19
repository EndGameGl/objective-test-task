using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Objective.PriceTracker.Api.Models.Database;

namespace Objective.PriceTracker.Api.Services.Database.EntityTypeConfigurations;

public class DbApartmentPriceTrackerEntityTypeConfiguration
    : IEntityTypeConfiguration<DbApartmentPriceTracker>
{
    public void Configure(EntityTypeBuilder<DbApartmentPriceTracker> builder)
    {
        builder.ToTable("apartment_price_trackers");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();

        builder.HasIndex(x => x.ApartmentId).IsUnique();
        builder.Property(x => x.ApartmentId).IsRequired();

        builder.Property(x => x.ProjectName).HasMaxLength(128).IsRequired(false);

        builder.Property(x => x.LastPrice).IsRequired(false);

        builder.Property(x => x.LastUpdated).IsRequired(false);

        builder
            .HasMany(x => x.Subscriptions)
            .WithOne(x => x.ApartmentTracker)
            .HasForeignKey(x => x.ApartmentTrackerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
