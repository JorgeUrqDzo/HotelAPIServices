using System;
using Hotel.Entities;
using Hotel.Services.Interfaces;
using Hotel.Services.Settings;

namespace Hotel.Services
{
    public class EventLog : IEventLog
    {
        IApplicationEventsService ApplicationEventsService { get; set; }
        IApplicationSettingsService ApplicationSettingsService { get; set; }
        SystemSettings ApplicationSettings { get; set; }

        public EventLog(IApplicationEventsService applicationEventsService, IApplicationSettingsService applicationSettingsService)
        {
            ApplicationEventsService = applicationEventsService;
            ApplicationSettingsService = applicationSettingsService;

            ApplicationSettings = TryLoadSettings() ?? new SystemSettings();
        }

        private SystemSettings TryLoadSettings()
        {
            try
            {
                return ApplicationSettingsService.GetSystemSettings();
            }
            catch
            {
                return null;
            }
        }

        public bool LogError(string controllerName, Exception exception)
        {
            if (ApplicationSettings.ApplicationLoggingEnabled == false)
            {
                return false;
            }

            try
            {
                ApplicationEventsService.Add(new ApplicationEvent
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    EventType = ApplicationEventTypes.Error,
                    Controller = controllerName
                });

            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}