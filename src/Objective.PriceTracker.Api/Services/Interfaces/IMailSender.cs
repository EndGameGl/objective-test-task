using Objective.PriceTracker.Api.Models.Mails;

namespace Objective.PriceTracker.Api.Services.Interfaces;

public interface IMailSender
{
    Task SendPriceNotificationUpdateAsync(MailPackage mailPackage);
}
