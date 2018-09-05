using System;

namespace Hotel.Services.Interfaces
{
    public interface IEventLog
    {
        bool LogError(string controllerName, Exception exception);
    }
}