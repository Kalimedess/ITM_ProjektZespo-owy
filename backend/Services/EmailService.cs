using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{

    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public bool EnableSSL { get; set; }
        public string SenderName { get; set; } = string.Empty;
        private string? _senderEmail;

    public string SenderEmail
        {
            get => !string.IsNullOrEmpty(_senderEmail) ? _senderEmail : SmtpUsername;
            set => _senderEmail = value;
        }
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        // Poprawny konstruktor
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, IConfiguration configuration)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
            _configuration = configuration;
        }

        // Poprawiona metoda, oznaczona jako `public async Task`
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);

                var mailMessage = new MailMessage
                {
                    From = fromAddress,
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                var alternateView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                var logoPath = Path.Combine(AppContext.BaseDirectory, "Templates", "ITM_poziom_biale.png");
                
                using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                    client.EnableSsl = _emailSettings.EnableSSL;

                    _logger.LogInformation("Wysyłanie e-maila do {To} z tematem {Subject}", to, subject);
                    await client.SendMailAsync(mailMessage); // 'await' działa, bo metoda jest 'async'
                    _logger.LogInformation("E-mail do {To} został pomyślnie wysłany.", to);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas wysyłania e-maila do {To}", to);
                // Celowo nie rzucamy wyjątku dalej
            }
        }

        public async Task SendConfirmationEmailAsync(string userEmail, string confirmationToken, DateTime? expireDate)
        {
            // Krok 1: Zdefiniuj ścieżkę do szablonu
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "ConfirmationEmail.html");

            if (!File.Exists(templatePath))
            {
                _logger.LogError("Nie znaleziono szablonu e-maila w ścieżce: {TemplatePath}", templatePath);
                // Można rzucić wyjątek lub po prostu zakończyć, aby nie wysłać pustego maila
                return;
            }

            // Krok 2: Wczytaj całą zawartość pliku szablonu
            var emailBody = await File.ReadAllTextAsync(templatePath);

            // Krok 3: Przygotuj dynamiczne dane do wstawienia
            var frontendBaseUrl =  Environment.GetEnvironmentVariable("FRONTEND_URL");
            var confirmationLink = $"{frontendBaseUrl}/confirm/{confirmationToken}";
            string expireDateString = $"{expireDate.Value:dd.MM.yyyy HH:mm}";

            // Krok 4: Podmień placeholdery w szablonie
            emailBody = emailBody.Replace("{{CONFIRMATION_LINK}}", confirmationLink);
            emailBody = emailBody.Replace("{{EXPIRE_DATE}}", expireDateString);

            var subject = "Potwierdź swoją rejestrację w Digital Wars";

            // Krok 5: Wyślij e-mail z wczytaną i zmodyfikowaną treścią
            await SendEmailAsync(userEmail, subject, emailBody);
        }

        public async Task SendPasswordResetEmailAsync(string userEmail, string resetToken, DateTime? expireDate)
        {
            // Krok 1: Zdefiniuj ścieżkę do szablonu
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "ResetPasswordEmail.html");

            if (!File.Exists(templatePath))
            {
                _logger.LogError("Nie znaleziono szablonu e-maila w ścieżce: {TemplatePath}", templatePath);
                // Można rzucić wyjątek lub po prostu zakończyć, aby nie wysłać pustego maila
                return;
            }

            // Krok 2: Wczytaj całą zawartość pliku szablonu
            var emailBody = await File.ReadAllTextAsync(templatePath);

            // Krok 3: Przygotuj dynamiczne dane do wstawienia

            var frontendBaseUrl =  Environment.GetEnvironmentVariable("FRONTEND_URL");
            var resetLink = $"{frontendBaseUrl}/resetPassword/{resetToken}";
            string expireDateString = $"{expireDate.Value:dd.MM.yyyy HH:mm}";

            // Krok 4: Podmień placeholdery w szablonie
            emailBody = emailBody.Replace("{{RESET_LINK}}", resetLink);
            emailBody = emailBody.Replace("{{EXPIRE_DATE}}", expireDateString);

            var subject = "Resetowanie hasła";

            // Krok 5: Wyślij e-mail z wczytaną i zmodyfikowaną treścią
            await SendEmailAsync(userEmail, subject, emailBody);
        }

    }
}
