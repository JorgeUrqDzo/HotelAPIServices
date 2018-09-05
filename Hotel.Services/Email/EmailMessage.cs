using System.Collections.Generic;

namespace Hotel.Services.Email
{
    public class EmailMessage
    {
        public List<EmailAddress> To { get; set; }
        public EmailAddress From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsHtml { get; set; }

        public EmailMessage()
        {
            To = new List<EmailAddress>();
        }
    }
}