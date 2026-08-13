using Microsoft.AspNetCore.Identity.UI.Services;

namespace Garage2._0.Services;

// TODO(#26): Ersätt med riktig email-sender vid US14 (lösenordsåterställning)
public class GarageEmailSender : IEmailSender
{
    private readonly ILogger<GarageEmailSender> _logger;

    public GarageEmailSender(ILogger<GarageEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation(
            "Would have sent email to {Email} with subject '{Subject}'",
            email, subject);
        return Task.CompletedTask;
    }
}
