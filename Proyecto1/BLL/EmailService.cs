using SendGrid;
using SendGrid.Helpers.Mail;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarAsync(string destinatario, string asunto, string mensaje)
    {
        var apiKey = _config["SendGrid:ApiKey"];
        var client = new SendGridClient(apiKey);

        var from = new EmailAddress(
            _config["SendGrid:FromEmail"],
            _config["SendGrid:FromName"]
        );

        var to = new EmailAddress(destinatario);

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            asunto,
            mensaje,
            mensaje
        );

        await client.SendEmailAsync(msg);
    }
}

