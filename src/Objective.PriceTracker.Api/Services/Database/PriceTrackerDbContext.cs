using Microsoft.EntityFrameworkCore;
using Objective.PriceTracker.Api.Models.Database;
using Objective.PriceTracker.Api.Services.Database.EntityTypeConfigurations;

namespace Objective.PriceTracker.Api.Services.Database;

public class PriceTrackerDbContext : DbContext
{
    public DbSet<DbApartmentPriceTracker> ApartmentPriceTrackers { get; set; }
    public DbSet<DbSubscription> Subscriptions { get; set; }
    public DbSet<DbSubscriber> Subscribers { get; set; }

    public PriceTrackerDbContext(DbContextOptions<PriceTrackerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("price_tracking");
        modelBuilder.ApplyConfiguration(new DbSubscriberEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DbSubscriptionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DbApartmentPriceTrackerEntityTypeConfiguration());
    }
}
