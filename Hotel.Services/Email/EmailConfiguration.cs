namespace Hotel.Services.Email
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string Server { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public EmailAddress SenderAddress { get; set; }
    }
}