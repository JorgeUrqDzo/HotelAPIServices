using System;
using Hotel.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.WebApi
{
    [Authorize(Policy = "PIPolicy")]
    [TokenUpdateFilter()]
    public class BaseController : Controller
    {
        private Guid userId;
        private Guid instanceId;

        public Guid UserID
        {
            get
            {
                if (userId == Guid.Empty)
                {
                    userId = Guid.Parse(User.FindFirst("UserId")?.Value);
                }
                return userId;
            }
        }

        public Guid InstanceID
        {
            get
            {
                if (instanceId == Guid.Empty)
                {
                    instanceId = Guid.Parse(User.FindFirst("InstanceId")?.Value);
                }
                return instanceId;
            }
        }

        protected IActionResult Unauthorized(string message, string code = null, object data = null)
        {
            var result = new JsonResult(new
            {
                ErrorCode = code,
                Message = message,
                Data = data
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            return result;
        }
    }
}