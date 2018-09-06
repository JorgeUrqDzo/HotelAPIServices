using System;

namespace Hotel.Services.Exceptions
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message)
            : base(message)
        {
        }
    }
}