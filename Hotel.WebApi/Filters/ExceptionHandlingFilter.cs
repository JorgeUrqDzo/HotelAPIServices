using System.Net;
using Hotel.Services.Interfaces;
using Hotel.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.WebApi.Filters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        IEventLog EventLog { get; }

        public ExceptionHandlingFilter(IEventLog eventLog)
        {
            EventLog = eventLog;
        }

        public void OnException(ExceptionContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (EventLog.LogError(actionDescriptor?.ControllerName, context.Exception))
            {
                context.ExceptionHandled = true;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Result = new JsonResult(new ErrorResponse());
            }
        }
    }
}