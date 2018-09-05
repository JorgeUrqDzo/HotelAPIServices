using System;
using Microsoft.AspNetCore.Authorization;

namespace Hotel.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params object[] roles)
        {
            this.Roles = string.Join(",", roles);
        }
    }
}