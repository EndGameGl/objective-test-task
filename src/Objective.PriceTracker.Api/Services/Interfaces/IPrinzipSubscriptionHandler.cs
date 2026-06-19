using Objective.PriceTracker.Api.Models;

namespace Objective.PriceTracker.Api.Services.Interfaces;

public interface IPrinzipSubscriptionHandler
{
    Task<SubscriptionResult> HandleSubscriptionAsync(string mail, string url);
}
