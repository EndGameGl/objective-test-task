using System.Text.Json.Serialization;

namespace Objective.PriceTracker.Api.Models.Prinzip;

public class PrinzipApartment
{
    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }

    [JsonPropertyName("status_type")]
    public string StatusType { get; set; }

    [JsonPropertyName("pricings")]
    public PrinzipApartmentPricing[] Pricings { get; set; }
}
