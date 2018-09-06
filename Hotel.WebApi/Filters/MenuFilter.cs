using Hotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.WebApi.Filters
{
    public class MenuFilter : IActionFilter
    {
        private readonly IMenuItemService service;

        public MenuFilter(IMenuItemService service)
        {
            this.service = service;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var endpoint = context.HttpContext.Request.Path.ToString();

            var controller = GetController(endpoint);

            if (!service.HasPermission(controller)) context.HttpContext.Response.StatusCode = 401;
        }

        private string GetController(string endPoint)
        {
            var uriParts = endPoint.Split('/');

            return uriParts[2];
        }
    }
}