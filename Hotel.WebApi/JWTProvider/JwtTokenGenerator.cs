using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Hotel.Entities;
using Hotel.Services.Settings;
using Hotel.WebApi.Formatters;

namespace Hotel.WebApi.JWTProvider
{
    public class JwtTokenGenerator
    {
        private readonly int ExpirationInMinutes;

        public JwtTokenGenerator(CustomSettings settings)
        {
            ExpirationInMinutes = settings.TokenExpiration;
            Settings = settings;
        }

        private CustomSettings Settings { get; }

        public JwtToken GenerateSecurityToken(User user, Guid? instanceOverride = null)
        {
            return new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(Settings.TokenKey))
                .AddSubject(user.UserName)
                .AddIssuer("HotelApi")
                .AddAudience("HotelApi")
                .AddClaim("UserId", user.Id.ToString())
                .AddClaim("UserFullName", FullNameFormatter.Format(user.FirstName, user.LastName, user.MiddleName))
                .AddClaim("InstanceId", (instanceOverride == null
                    ? user.InstanceId
                    : instanceOverride).ToString())
                .AddClaim(ClaimTypes.Role, user.Role.Identifier)
                .AddExpiry(ExpirationInMinutes)
                .Build();
        }

        public JwtToken RefreshSecurityToken(JwtSecurityToken existingToken)
        {
            var userId = existingToken.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var instanceId = existingToken.Claims.FirstOrDefault(c => c.Type == "InstanceId")?.Value;
            var fullName = existingToken.Claims.FirstOrDefault(c => c.Type == "UserFullName")?.Value;
            var role = existingToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(Settings.TokenKey))
                .AddSubject(existingToken.Subject)
                .AddIssuer("HotelApi")
                .AddAudience("HotelApi")
                .AddClaim("UserId", userId)
                .AddClaim("UserFullName", fullName ?? string.Empty)
                .AddClaim("InstanceId", instanceId)
                .AddClaim(ClaimTypes.Role, role)
                .AddExpiry(ExpirationInMinutes)
                .Build();
        }
    }
}