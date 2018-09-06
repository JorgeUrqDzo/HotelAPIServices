using System;
using Hotel.Entities;
using Hotel.Services.Interfaces;
using Hotel.UnitOfWork;

namespace Hotel.Services
{
    public class ApplicationEventsService : BaseService, IApplicationEventsService
    {
        readonly IRepository<ApplicationEvent> repository;

        public ApplicationEventsService(IUnitOfWork unitOfWork, IAppContext context)
            : base(unitOfWork, context)
        {
            repository = unitOfWork.GetRepository<ApplicationEvent>();
        }

        public void Add(ApplicationEvent applicationEvent)
        {
            applicationEvent.Id = Guid.NewGuid();
            applicationEvent.EventDate = DateTimeProvider.UtcNow;
            applicationEvent.InstanceId = AppContext.IsValid ? (Guid?) AppContext.InstanceId : null;
            applicationEvent.UserId = AppContext.IsValid ? (Guid?) AppContext.UserId : null;
            applicationEvent.UserIPAddress = AppContext.UserIpAddress;
            applicationEvent.ServerName = AppContext.ServerName;

            repository.Insert(applicationEvent);

            UnitOfWork.SaveChanges();
        }
    }
}