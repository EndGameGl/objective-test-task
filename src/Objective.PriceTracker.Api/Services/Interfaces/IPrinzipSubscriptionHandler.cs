using Objective.PriceTracker.Api.Models;
using Objective.PriceTracker.Api.Models.Responses;

namespace Objective.PriceTracker.Api.Services.Interfaces;

public interface IPrinzipSubscriptionHandler
{
    Task<SubscriptionResult> HandleSubscriptionAsync(
        string mail,
        string url,
        CancellationToken cancellation
    );
    Task<List<PrinzipApartmentPrice>> GetCurrentSubscribedApartments(
        string mail,
        CancellationToken cancellation
    );
}
