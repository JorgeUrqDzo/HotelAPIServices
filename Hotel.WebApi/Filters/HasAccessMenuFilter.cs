using Hotel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hotel.WebApi.Filters
{
    public class HasAccesMenuFilter : ActionFilterAttribute
    {
        private readonly IMenuItemService _dataService;

        public HasAccesMenuFilter(IMenuItemService dataService)
        {
            _dataService = dataService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}