using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.RegularExpressions;

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
        // Extrahera href-URL:en från HTML för enklare läsning i dev
        var match = Regex.Match(htmlMessage, @"href='([^']+)'");
        var url = match.Success ? match.Groups[1].Value : "(ingen länk hittades)";

        _logger.LogInformation(
            "📧 DEV EMAIL till {Email} | Ämne: {Subject}\n🔗 Länk: {Url}",
            email, subject, url);

        return Task.CompletedTask;
    }
}
