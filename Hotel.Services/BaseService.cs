using Hotel.Entities;
using Hotel.Services.Interfaces;
using Hotel.UnitOfWork;

namespace Hotel.Services
{
    public abstract class BaseService
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        protected IAppContext AppContext { get; private set; }
        protected bool implicitAccess;

        protected BaseService(IUnitOfWork unitOfWork, IAppContext context)
        {
            UnitOfWork = unitOfWork;
            AppContext = context;

//            implicitAccess = RolesIdentifiers.IsAdmin(AppContext.Role);
        }
    }
}