using System.Text.Json.Serialization;

namespace Objective.PriceTracker.Api.Models.Responses;

public class PrinzipApartmentPrice
{
    [JsonPropertyName("url")]
    public string ApartmentUrl { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }
}
