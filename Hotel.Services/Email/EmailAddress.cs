namespace Hotel.Services.Email
{
    public class EmailAddress
    {
        public EmailAddress()
        {
        }

        public EmailAddress(string address)
        {
            Address = Name = address;
        }

        public EmailAddress(string address, string name)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; set; }
        public string Address { get; set; }
    }
}