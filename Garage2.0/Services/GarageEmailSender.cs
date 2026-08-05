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
        "Skulle skickat mail till {Email} med ämne '{Subject}'",
        email, subject);
    return Task.CompletedTask;
  }
}
