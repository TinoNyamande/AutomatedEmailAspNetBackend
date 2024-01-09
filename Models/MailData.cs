
namespace EmailAPI.Models
{
    public class MailData
    {
        public string? To { get; }
        public string? ReplyTo { get; }
        public string? ReplyName { get; }
        public string Subject { get; }

        public string? Body { get; }

        public MailData(string to , string subject ,string body,string replyTo = null,string replyName = null)
        {
            To = to;
            Subject = subject;
            Body = body;
            ReplyTo = replyTo;
            ReplyName = replyName;
        }
    }
}
