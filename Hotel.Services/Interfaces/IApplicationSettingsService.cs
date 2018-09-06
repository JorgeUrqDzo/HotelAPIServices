using Hotel.Services.Settings;

namespace Hotel.Services.Interfaces
{
    public interface IApplicationSettingsService
    {
        T GetApplicationSettings<T>(string settingsType) where T : class, new();
        UserAccountSettings GetUserAccountSettings();
        SystemSettings GetSystemSettings();
    }
}