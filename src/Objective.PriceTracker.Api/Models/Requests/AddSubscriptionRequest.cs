using System.Text.Json.Serialization;

namespace Objective.PriceTracker.Api.Models.Requests;

public class AddSubscriptionRequest
{
    /// <summary>
    ///     Link to the parsed page
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;

    /// <summary>
    ///     Mail to send changes to
    /// </summary>
    [JsonPropertyName("mail")]
    public string Mail { get; set; } = null!;
}
