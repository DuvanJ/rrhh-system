using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarAsync(string destinatario, string asunto, string mensaje)
    {
        var smtp = new SmtpClient(_config["EmailSettings:Host"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]),
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _config["EmailSettings:UserName"],
                _config["EmailSettings:Password"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(
                _config["EmailSettings:From"],
                "Sistema RRHH"
            ),
            Subject = asunto,
            Body = mensaje,
            IsBodyHtml = true
        };

        mail.To.Add(destinatario);

        await smtp.SendMailAsync(mail);
    }
}