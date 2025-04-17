namespace ProjectService.Services.MailService
{
    public interface IMailService
    {
        Task<bool> SendAsync(MailData mailData);
    }
}
