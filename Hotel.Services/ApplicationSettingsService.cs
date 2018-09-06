using System.Linq;
using Hotel.Entities;
using Hotel.Services.Extensions;
using Hotel.Services.Interfaces;
using Hotel.Services.Settings;
using Hotel.UnitOfWork;

namespace Hotel.Services
{
    public class ApplicationSettingsService : BaseService, IApplicationSettingsService
    {
        IRepository<ApplicationSetting> AppSettingsRepository;

        public ApplicationSettingsService(IUnitOfWork unitOfWork, IAppContext context) : base(unitOfWork, context)
        {
            AppSettingsRepository = unitOfWork.GetRepository<ApplicationSetting>();
        }

        public T GetApplicationSettings<T>(string settingsType) where T : class, new()
        {
            var settings = AppSettingsRepository
                .GetPagedList(predicate: s => s.Type == settingsType)
                .Items
                .ToDictionary(s => s.Name, s => s.Value);

            return settings.ToObject<T>();
        }
        
        public UserAccountSettings GetUserAccountSettings()
        {
            return GetApplicationSettings<UserAccountSettings>(ApplicationSettingsTypes.UserAccount);
        }

        public SystemSettings GetSystemSettings()
        {
            return GetApplicationSettings<SystemSettings>(ApplicationSettingsTypes.System);
        }
    }
}