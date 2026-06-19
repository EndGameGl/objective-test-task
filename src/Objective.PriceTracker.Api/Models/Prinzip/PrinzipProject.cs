using System.Text.Json.Serialization;

namespace Objective.PriceTracker.Api.Models.Prinzip;

public class PrinzipProject
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
}
