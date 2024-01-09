using EmailAPI.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using MimeKit;

namespace EmailAPI.Services
{
    public class EmailService :IEmailService
    {
        private readonly MailSettings _settings;
        public EmailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<String> sendEmailAsync (MailData mailData , CancellationToken ct,string attachmentFilePath)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(
                _settings.DisplayName, _settings.From));
            mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);
            mail.To.Add(MailboxAddress.Parse(mailData.To));
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            var attachmentPath = "C:/Users/tnyamande/source/repos/EMS/EMS/wwwroot/Expiring Zimra Tax Clearance.pdf";
            var attachmentFileName = "Test.pdf";
            body.Attachments.Add(attachmentFilePath);
            mail.Body = body.ToMessageBody();
            try
            {
                using var smtp = new SmtpClient();
                if (_settings.Usessl)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTIs)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);
                return "Email sent successfully";

            }catch(Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
