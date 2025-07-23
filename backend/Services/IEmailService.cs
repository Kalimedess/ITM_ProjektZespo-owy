namespace backend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendConfirmationEmailAsync(string userEmail, string confirmationToken, DateTime? dateTime);
        // DODAJ NOWĄ METODĘ
        Task SendPasswordResetEmailAsync(string userEmail, string resetToken, DateTime? dateTime);
    }
}