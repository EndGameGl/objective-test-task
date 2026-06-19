using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Objective.PriceTracker.Api.Models;
using Objective.PriceTracker.Api.Models.Database;
using Objective.PriceTracker.Api.Services.Database;
using Objective.PriceTracker.Api.Services.Interfaces;

namespace Objective.PriceTracker.Api.Services;

public partial class PrinzipSubscriptionHandler : IPrinzipSubscriptionHandler
{
    [GeneratedRegex("https://prinzip.su/flats/(?<complexName>[0-9a-z_-]+)/(?<apartmentId>[0-9]+)/")]
    private partial Regex UrlRegex { get; }

    private readonly ILogger<PrinzipSubscriptionHandler> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PrinzipSubscriptionHandler(
        ILogger<PrinzipSubscriptionHandler> logger,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<SubscriptionResult> HandleSubscriptionAsync(string mail, string url)
    {
        var match = UrlRegex.Match(url);

        if (!match.Success)
        {
            _logger.LogWarning("Invalid url despite validation passed: {Url}", url);
            return SubscriptionResult.Error;
        }

        if (!match.Groups.TryGetValue("apartmentId", out var apartmentIdGroup))
        {
            _logger.LogWarning("Regex match didn't have apartmentId group: {Url}", url);
            return SubscriptionResult.Error;
        }

        if (!int.TryParse(apartmentIdGroup.ValueSpan, out var apartmentId))
        {
            _logger.LogWarning(
                "Couldn't parse apartment Id: {ApartmentId}",
                apartmentIdGroup.Value
            );
            return SubscriptionResult.Error;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PriceTrackerDbContext>();

        // fetch or add subscribing user reference
        var subscriber = (
            await dbContext
                .Subscribers.Upsert(new DbSubscriber() { SubscriberMail = mail })
                .On(x => x.SubscriberMail)
                .WhenMatched((x, xi) => new DbSubscriber() { SubscriberMail = xi.SubscriberMail })
                .RunAndReturnAsync()
        ).Single();

        // fetch or add price tracker for current apartment id
        var priceTracker = (
            await dbContext
                .ApartmentPriceTrackers.Upsert(
                    new DbApartmentPriceTracker()
                    {
                        ApartmentId = apartmentId,
                        LastPrice = null,
                        LastUpdated = null,
                    }
                )
                .On(x => x.ApartmentId)
                .WhenMatched(
                    (x, xi) => new DbApartmentPriceTracker() { ApartmentId = xi.ApartmentId }
                )
                .RunAndReturnAsync()
        ).Single();

        dbContext.Subscriptions.Add(
            new DbSubscription() { ApartmentTracker = priceTracker, Subscriber = subscriber }
        );

        try
        {
            await dbContext.SaveChangesAsync();
            return SubscriptionResult.Success;
        }
        catch (DbUpdateException dbUpdateException)
        {
            _logger.LogWarning(
                dbUpdateException,
                "Failed to insert new subscription: {Mail}, Apartment: {ApartmentId}",
                subscriber.SubscriberMail,
                apartmentId
            );

            return SubscriptionResult.Error;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Unhandled error while inserting new subscription: {Mail}, Apartment: {ApartmentId}",
                subscriber.SubscriberMail,
                apartmentId
            );

            return SubscriptionResult.Error;
        }
    }
}
