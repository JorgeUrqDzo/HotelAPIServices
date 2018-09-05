using System;
using System.Net;
using System.Net.Mail;

namespace Hotel.Services.Email
{
    public class EmailService : IEmailService
    {
        public IEmailConfiguration EmailConfiguration { get; private set; }

        public EmailService(IEmailConfiguration configuration)
        {
            EmailConfiguration = configuration;
        }

        public void Send(EmailMessage message)
        {
            var sender = new MailAddress(EmailConfiguration.SenderAddress.Address,
                EmailConfiguration.SenderAddress.Name);

            var mailMessage = new MailMessage
            {
                From = message.From != null ? new MailAddress(message.From.Address, message.From.Name) : sender,
                Subject = message.Subject,
                Body = message.Content,
                IsBodyHtml = message.IsHtml,
                Sender = sender
            };

            message.To.ForEach(a => mailMessage.To.Add(new MailAddress(a.Address, a.Name)));

            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EmailConfiguration.UserName, EmailConfiguration.Password),
                Host = EmailConfiguration.Server,
                EnableSsl = EmailConfiguration.UseSsl
            };

            if (EmailConfiguration.Port.HasValue)
            {
                smtpClient.Port = EmailConfiguration.Port.Value;
            }

            smtpClient.Send(mailMessage);
        }

        public void Send(IEmailTemplate emailTemplate)
        {
            if (emailTemplate == null)
            {
                throw new ArgumentNullException("emailTemplate");
            }

            Send(emailTemplate.BuildMessage());
        }
    }
}