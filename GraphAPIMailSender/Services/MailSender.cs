using Azure.Identity;
using GraphAPIMailSender.Interfaces;
using GraphAPIMailSender.Models;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;

namespace GraphAPIMailSender.Services
{
    public class MailSender : IMailSender
    {
        private readonly GraphAPIKeys _graphAPIKeys;
        private readonly ILogger<MailSender> _logger;

        public MailSender(IOptions<GraphAPIKeys> options, ILogger<MailSender> logger)
        {
            _graphAPIKeys = options.Value;
            _logger = logger;
        }

        public async Task SendEmail(MailSenderDetails mailSenderDetails)
        {
            try
            {
                string? tenantId = _graphAPIKeys.TenantId;
                string? clientId = _graphAPIKeys.ClientId;
                string? clientSecret = _graphAPIKeys.ClientSecret;

                ClientSecretCredential credential = new(tenantId, clientId, clientSecret);
                GraphServiceClient graphClient = new(credential);

                var body = File.ReadAllText(_graphAPIKeys.HtmlPath)
                    .Replace("{{BODY}}", $"{mailSenderDetails.Body}.");
                body = !string.IsNullOrWhiteSpace(mailSenderDetails.Body) ? body.Replace("{dynamicContent}", mailSenderDetails.Body) :
                    body.Replace("{dynamicContent}", "Please refer to the attached files. Thank you for choosing Precision Glass Industries. We are dedicated to providing high-quality glass products and exceptional service.");

                var toEmails = EmailGetter(mailSenderDetails.ToEmails);
                var ccRecipients = EmailGetter(mailSenderDetails.CcRecipients);
                var bccRecipients = EmailGetter(mailSenderDetails.BccRecipients);
                var attachments = AttachmentGetter(mailSenderDetails.FileDetails);

                var requestBody = new SendMailPostRequestBody
                {
                    Message = new Message
                    {
                        Subject = mailSenderDetails.Subject,
                        Body = new ItemBody
                        {
                            ContentType = BodyType.Html,
                            Content = body,
                        },
                        ToRecipients = toEmails,
                        CcRecipients = ccRecipients,
                        BccRecipients = bccRecipients,
                        Attachments = attachments
                    }
                };
                await graphClient.Users[_graphAPIKeys.FromEmail].SendMail.PostAsync(requestBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public List<Recipient> EmailGetter(List<Mail> mails)
        {
            var recipients = new List<Recipient>();
            if (mails.Count > 0)
                foreach (var item in mails)
                {
                    recipients.Add(new Recipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = item.Email
                        }
                    });
                }
            return recipients;
        }

        public List<Attachment> AttachmentGetter(List<FileDetails> fileDetails)
        {
            var attachments = new List<Attachment>();
            if (fileDetails.Count > 0)
                foreach (var item in fileDetails)
                {
                    attachments.Add(new FileAttachment
                    {
                        OdataType = "#microsoft.graph.fileAttachment",
                        ContentBytes = File.ReadAllBytes(item.FilePath),
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        Name = item.FileName,
                    });
                }
            return attachments;
        }
    }
}
