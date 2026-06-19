using System.Text.Json.Serialization;

namespace Objective.PriceTracker.Api.Models.Prinzip;

public class PrinzipApartmentPricing
{
    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; } = null!;

    [JsonPropertyName("price_base")]
    public double BasePrice { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("published_to")]
    public DateTime DatePublished { get; set; }
}
