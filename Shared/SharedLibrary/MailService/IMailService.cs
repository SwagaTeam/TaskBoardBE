public interface IMailService
{
    Task<bool> SendAsync(MailData mailData);
}