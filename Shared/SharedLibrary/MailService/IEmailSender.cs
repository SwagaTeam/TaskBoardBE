public interface IEmailSender
{
    Task<bool> SendEmailAsync(string subject, string message, params string[] to);
}