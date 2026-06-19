namespace Objective.PriceTracker.Api.Models.Database;

public class DbSubscription
{
    public long Id { get; set; }

    public long SubscriberId { get; set; }
    public DbSubscriber Subscriber { get; set; } = null!;

    public long ApartmentTrackerId { get; set; }

    public DbApartmentPriceTracker ApartmentTracker { get; set; } = null!;
}
