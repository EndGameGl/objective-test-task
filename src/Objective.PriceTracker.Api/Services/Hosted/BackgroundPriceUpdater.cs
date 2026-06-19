using Microsoft.EntityFrameworkCore;
using Objective.PriceTracker.Api.Models.Mails;
using Objective.PriceTracker.Api.Services.Database;
using Objective.PriceTracker.Api.Services.Interfaces;

namespace Objective.PriceTracker.Api.Services.Hosted;

public class BackgroundPriceUpdater : BackgroundService
{
    private readonly IPrinzipApi _prinzipApi;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IMailSender _mailSender;

    public BackgroundPriceUpdater(
        IPrinzipApi prinzipApi,
        IServiceScopeFactory serviceScopeFactory,
        IMailSender mailSender
    )
    {
        _prinzipApi = prinzipApi;
        _serviceScopeFactory = serviceScopeFactory;
        _mailSender = mailSender;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return UpdateLoop(stoppingToken);
    }

    private async Task UpdateLoop(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5), TimeProvider.System);

        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PriceTrackerDbContext>();

            var priceTrackers = await dbContext
                .ApartmentPriceTrackers.Where(x => x.Subscriptions.Any())
                .ToListAsync();

            foreach (var priceTracker in priceTrackers)
            {
                var apartmentData = await _prinzipApi.GetApartmentDataAsync(
                    priceTracker.ApartmentId,
                    cancellationToken
                );

                // api didn't return valid response
                if (apartmentData is null)
                    continue;
                var pricing =
                    apartmentData.Pricings.FirstOrDefault(x => x.PaymentMethod == "full")
                    ?? apartmentData.Pricings.FirstOrDefault(x => x.PaymentMethod == "mortgage");

                // no known pricing found
                if (pricing is null)
                    continue;
                if (priceTracker.LastPrice != pricing.Price)
                {
                    var wasInitialLoad = !priceTracker.LastPrice.HasValue;
                    var oldPrice = priceTracker.LastPrice.GetValueOrDefault();
                    priceTracker.LastPrice = pricing.Price;
                    priceTracker.LastUpdated = DateTime.UtcNow;
                    if (string.IsNullOrEmpty(priceTracker.ProjectName))
                    {
                        var projectData = await _prinzipApi.GetProjectDataAsync(
                            apartmentData.ProjectId,
                            cancellationToken
                        );
                        priceTracker.ProjectName = projectData!.Url;
                    }

                    dbContext.ApartmentPriceTrackers.Update(priceTracker);
                    if (!wasInitialLoad)
                    {
                        var subcriberMails = await dbContext
                            .Subscriptions.Where(x => x.ApartmentTracker == priceTracker)
                            .Select(x => x.Subscriber.SubscriberMail)
                            .ToArrayAsync();

                        await _mailSender.SendPriceNotificationUpdateAsync(
                            new MailPackage()
                            {
                                Recipients = subcriberMails,
                                Content = $"""
                                Apartment: https://prinzip.su/flats/{priceTracker!.ProjectName}/{priceTracker.ApartmentId}/

                                Old price: {oldPrice}
                                New price: {pricing.Price}
                                """,
                            }
                        );
                    }
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
