using System;
using System.IdentityModel.Tokens.Jwt;
using Hotel.WebApi.JWTProvider;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.WebApi.Filters
{
    public class TokenUpdateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.HttpContext.Request.Headers.TryGetValue("Authorization", out var encodedToken)
               && string.IsNullOrWhiteSpace(encodedToken) == false)
            {
                var tokenGenerator = context.HttpContext.RequestServices.GetService<JwtTokenGenerator>();

                var token = new JwtSecurityToken(encodedToken.ToString().Replace("bearer ", "", StringComparison.CurrentCultureIgnoreCase));

                var updatedToken = tokenGenerator.RefreshSecurityToken(token);

                context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "X-Auth-Token");
                context.HttpContext.Response.Headers.Add("X-Auth-Token", updatedToken.Value);
            }
            else
            {
                base.OnActionExecuted(context);
            }
        }
    }
}