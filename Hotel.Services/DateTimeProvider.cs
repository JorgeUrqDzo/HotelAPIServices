using System;
using Hotel.Services.Interfaces;

namespace Hotel.Services
{
    public static class DateTimeProvider
    {
        private static IDateTimeProvider provider;
        public static IDateTimeProvider Provider
        {
            get
            {
                if (provider == null)
                {
                    provider = new DefaultDateTimeProvider();
                }

                return provider;
            }
            set { provider = value; }
        }

        public static DateTime Now
        {
            get { return Provider.Now(); }
        }

        public static DateTime Today
        {
            get { return Provider.Now().Date; }
        }

        public static DateTime UtcNow
        {
            get { return Provider.UtcNow(); }
        }
    }
}