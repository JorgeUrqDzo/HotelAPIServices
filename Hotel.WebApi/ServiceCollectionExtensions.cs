using System;
using System.Threading.Tasks;
using Hotel.Services.Email;
using Hotel.Services.Settings;
using Hotel.WebApi.JWTProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.WebApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEmailConfiguration>(configuration.GetSection("Email").Get<EmailConfiguration>())
                .AddTransient<IEmailService, EmailService>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            CustomSettings customSettings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = "HotelApi",
                            ValidAudience = "HotelApi",
                            IssuerSigningKey =
                                JwtSecurityKey.Create(customSettings.TokenKey),
                            ClockSkew = TimeSpan.Zero
                        };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PIPolicy",
                    policy => policy.RequireClaim("UserId")
                        .RequireClaim("InstanceId")
                        .RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                        .RequireAssertion(a => a.User.HasClaim(c => c.Issuer == "HotelApi"))
                        .RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
            });

            return services;
        }
    }
}