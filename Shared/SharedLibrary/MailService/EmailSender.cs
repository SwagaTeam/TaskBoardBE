public class EmailSender : IEmailSender
{
    private readonly IMailService mailService;

    public EmailSender(IMailService mailService)
    {
        this.mailService = mailService;
    }

    public async Task<bool> SendEmailAsync(string email, string subject, string message)
    {
        var mailData = new MailData(
            to: new List<string> { email },
            subject: subject,
            body: message,
            from: "TestMessagesService@yandex.ru",
            displayName: "TaskBoard",
            bcc: new List<string> { "TestMessagesService@yandex.ru" },
            cc: new List<string> { "TestMessagesService@yandex.ru" }
        );

        bool result = await mailService.SendAsync(mailData);

        return result;
    }
}