namespace Hotel.Services.Email
{
    public interface IEmailTemplate
    {
        EmailMessage BuildMessage();
    }
}