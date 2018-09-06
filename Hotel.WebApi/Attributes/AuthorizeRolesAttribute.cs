using System;
using Microsoft.AspNetCore.Authorization;

namespace Hotel.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params object[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}