using System;

namespace Hotel.Services.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public string ErrorCode { get; set; }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}