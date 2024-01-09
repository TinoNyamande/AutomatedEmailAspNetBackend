using EmailAPI.Models;

namespace EmailAPI.Services
{
    public interface IEmailService
    {
        Task<String> sendEmailAsync(MailData mailData,CancellationToken ct,string attachmentFilePath);
    }
}
