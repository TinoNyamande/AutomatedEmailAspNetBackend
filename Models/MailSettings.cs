namespace EmailAPI.Models
{
    public class MailSettings
    {
        public string? DisplayName { get; set; }
        public string? From { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int Port { get; set; }
        public string? Host { get; set; }
        public bool Usessl { get; set; }
        public bool UseStartTIs { get; set; }

    }
}
