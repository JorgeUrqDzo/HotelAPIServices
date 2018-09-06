using System.Net;
using Hotel.Entities;
using Hotel.Services;
using Hotel.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.WebApi.Filters
{
    public class WritePermissionsRequiredFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context
                .HttpContext
                .User
                .FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                ?.Value;

            if (RolesIdentifiers.IsReadOnly(role))
            {
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                context.Result = new JsonResult(new ErrorResponse(ApplicationMessages.WritePermissionsRequired));
            }
        }
    }
}