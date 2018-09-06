using System;
using Hotel.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hotel.WebApi
{
    public class AppContext : IAppContext
    {
        public AppContext(IHttpContextAccessor contextAccessor)
        {
            HttpContext = contextAccessor.HttpContext;

            IsValid = HttpContext != null && HttpContext.User.FindFirst("UserId") != null;
        }

        private HttpContext HttpContext { get; }

        public Guid UserId => new Guid(HttpContext.User.FindFirst("UserId")?.Value);

        public Guid InstanceId
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.User.FindFirst("InstanceId")?.Value))
                    return new Guid(HttpContext.User.FindFirst("InstanceId")?.Value);
                return Guid.Empty;
            }
        }

        public string Role => HttpContext.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            ?.Value;

        public string UserIpAddress => HttpContext.Connection.RemoteIpAddress.ToString();

        public string ServerName => Environment.MachineName;

        public string SourceUrl
        {
            get
            {
                var uri = new Uri(HttpContext.Request.Headers["Referer"].ToString());

                if (uri.Segments.Length > 1)
                    return uri.Segments[1] + (uri.Segments.Length > 2 ? uri.Segments[2] : string.Empty);
                return uri.AbsolutePath;
            }
        }

        public bool IsValid { get; }
    }
}