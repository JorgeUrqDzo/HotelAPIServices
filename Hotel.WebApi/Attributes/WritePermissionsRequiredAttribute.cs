using Hotel.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.WebApi.Attributes
{
    public class WritePermissionsRequiredAttribute : TypeFilterAttribute
    {
        public WritePermissionsRequiredAttribute()
            : base(typeof(WritePermissionsRequiredFilter))
        {
        }
    }
}