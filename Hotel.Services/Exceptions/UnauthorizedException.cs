using System;

namespace Hotel.Services.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; }
    }
}