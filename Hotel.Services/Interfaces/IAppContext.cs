using System;

namespace Hotel.Services.Interfaces
{
    public interface IAppContext
    {
        bool IsValid { get; }
        Guid UserId { get; }
        Guid InstanceId { get; }
        string Role { get; }
        string UserIpAddress { get; }
        string SourceUrl { get; }
        string ServerName { get; }
    }
}