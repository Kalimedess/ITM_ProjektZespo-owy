using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace backend.Services {

    public class EmailSettings {
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public bool EnableSSL { get; set; }
}

public class EmailService {
    private readonly EmailSettings _emailSettings;

    public EmailService(EmailSettings emailSettings) {
        _emailSettings = emailSettings;
    }

    public async Task SendEmailAsync(string to, string subject, string body) {
        using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)) {
            client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            client.EnableSsl = _emailSettings.EnableSSL;

    
        var mailMessage = new MailMessage {
            From = new MailAddress(_emailSettings.SmtpUsername),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);
        await client.SendMailAsync(mailMessage);
    }
    }


}
}
