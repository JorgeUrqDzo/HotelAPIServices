namespace Hotel.Services.Email
{
    public interface IEmailService
    {
        IEmailConfiguration EmailConfiguration { get; }
        void Send(EmailMessage message);
        void Send(IEmailTemplate emailTemplate);
    }
}