using System;
using Hotel.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hotel.WebApi
{
    public class AppContext : IAppContext
    {
        private HttpContext HttpContext { get; set; }

        public AppContext(IHttpContextAccessor contextAccessor)
        {
            HttpContext = contextAccessor.HttpContext;

            IsValid = (HttpContext != null && HttpContext.User.FindFirst("UserId") != null);
        }

        public Guid UserId
        {
            get { return new Guid(HttpContext.User.FindFirst("UserId")?.Value); }
        }

        public Guid InstanceId
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.User.FindFirst("InstanceId")?.Value))
                {
                    return new Guid(HttpContext.User.FindFirst("InstanceId")?.Value);
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        public string Role
        {
            get
            {
                return HttpContext.User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    ?.Value;
            }
        }

        public string UserIpAddress
        {
            get { return HttpContext.Connection.RemoteIpAddress.ToString(); }
        }

        public string ServerName
        {
            get { return Environment.MachineName; }
        }

        public string SourceUrl
        {
            get
            {
                var uri = new Uri(HttpContext.Request.Headers["Referer"].ToString());

                if (uri.Segments.Length > 1)
                {
                    return uri.Segments[1] + (uri.Segments.Length > 2 ? uri.Segments[2] : string.Empty);
                }
                else
                {
                    return uri.AbsolutePath;
                }
            }
        }

        public bool IsValid { get; private set; }
    }
}