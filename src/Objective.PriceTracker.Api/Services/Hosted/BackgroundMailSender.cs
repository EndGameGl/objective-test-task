using System.Threading.Channels;
using MailKit.Net.Smtp;
using MimeKit;
using Objective.PriceTracker.Api.Models.Mails;
using Objective.PriceTracker.Api.Services.Interfaces;

namespace Objective.PriceTracker.Api.Services.Hosted;

public class BackgroundMailSender : BackgroundService, IMailSender
{
    private readonly Channel<MailPackage> _mailQueue;
    private readonly IConfiguration _configuration;

    public BackgroundMailSender(IConfiguration configuration)
    {
        _mailQueue = Channel.CreateUnbounded<MailPackage>();
        _configuration = configuration;
    }

    public async Task SendPriceNotificationUpdateAsync(MailPackage mailPackage)
    {
        await _mailQueue.Writer.WriteAsync(mailPackage);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return MailSendLoop(stoppingToken);
    }

    private async Task MailSendLoop(CancellationToken cancellationToken)
    {
        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            host: _configuration.GetValue<string>("Mailpit:Host")!,
            port: _configuration.GetValue<int>("Mailpit:Port")!,
            cancellationToken: cancellationToken
        );

        await foreach (var mail in _mailQueue.Reader.ReadAllAsync(cancellationToken))
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Objective", "objective@test.com"));
            for (int i = 0; i < mail.Recipients.Length; i++)
            {
                message.To.Add(new MailboxAddress(null, mail.Recipients[i]));
            }
            message.Subject = "Apartment price was updated!";
            message.Body = new TextPart("plain") { Text = mail.Content };
            await smtpClient.SendAsync(message, cancellationToken);
        }
    }
}
