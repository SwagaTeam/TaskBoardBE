using MailKit.Net.Smtp;
using MimeKit;


namespace SharedLibrary.MailService
{
    public static class MailService
    {
        public static async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TaskBoard", "TestMessagesService@yandex.ru"));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.yandex.ru", 465, true);
            await client.AuthenticateAsync("TestMessagesService@yandex.ru", "ssdfhpurlhurttzk");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
