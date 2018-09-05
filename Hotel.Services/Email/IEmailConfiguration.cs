namespace Hotel.Services.Email
{
    public interface IEmailConfiguration
    {
        string Server { get; set; }
        int? Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        bool UseSsl { get; set; }
        EmailAddress SenderAddress { get; set; }
    }
}