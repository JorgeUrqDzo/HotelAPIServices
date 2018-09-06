using Hotel.Entities;

namespace Hotel.Services.Interfaces
{
    public interface IApplicationEventsService
    {
        void Add(ApplicationEvent applicationEvent);
    }
}