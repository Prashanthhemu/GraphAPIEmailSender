using GraphAPIMailSender.Models;

namespace GraphAPIMailSender.Interfaces
{
    public interface IMailSender
    {
        public Task SendEmail(MailSenderDetails mailSenderDetails);
    }
}
