namespace Hotel.Services.Exceptions
{
    public class PasswordExpiredException : UnauthorizedException
    {
        public PasswordExpiredException()
            : base("Your password has expired. Please update it to continue using the application.", "password_expired")
        {
        }
    }
}