using System;

namespace Hotel.Services.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now();
        DateTime UtcNow();
    }
}