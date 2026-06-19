namespace Objective.PriceTracker.Api.Models.Database;

public class DbApartmentPriceTracker
{
    public long Id { get; set; }

    public int ApartmentId { get; set; }
    public string? ProjectName { get; set; }

    public double? LastPrice { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual List<DbSubscription> Subscriptions { get; } = [];
}
