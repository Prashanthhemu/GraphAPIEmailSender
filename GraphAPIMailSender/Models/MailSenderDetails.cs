namespace GraphAPIMailSender.Models
{
    public class MailSenderDetails
    {
        public List<Mail> ToEmails { get; set; } = new List<Mail>();
        public string? Subject { get; set; } = string.Empty;
        public string? Body { get; set; } = string.Empty;
        public List<Mail> CcRecipients { get; set; } = new List<Mail>();
        public List<Mail> BccRecipients { get; set; } = new List<Mail>();
        public List<FileDetails> FileDetails { get; set; } = new List<FileDetails>();
    }

    public class Mail
    {
        public string Email { get; set; } = string.Empty;
    }

    public class FileDetails
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}
