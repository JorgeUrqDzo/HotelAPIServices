using Hotel.Data;
using Hotel.Services;
using Hotel.Services.Interfaces;
using Hotel.Services.Settings;
using Hotel.UnitOfWork;
using Hotel.WebApi.Filters;
using Hotel.WebApi.JWTProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Hotel.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => { options.Filters.Add(new TypeFilterAttribute(typeof(ExceptionHandlingFilter))); })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var customSettings = Configuration.GetSection("Keys").Get<CustomSettings>();

            services.AddJwtAuthentication(customSettings)
                .AddCors()
                .AddDbContext<HotelDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("HotelApiServices")))
                .AddScoped<IUnitOfWork, UnitOfWork<HotelDbContext>>()
                .AddEmailService(Configuration)
                .AddScoped<IAppContext, AppContext>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton(customSettings)
                .AddSingleton(Configuration.GetSection("ConnectionStrings").Get<ConnectionSettings>())
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<JwtTokenGenerator>()
                .AddScoped<IEventLog, EventLog>()
                .AddTransient<SecurityService>()
                .AddTransient<UsersService>()
                .AddTransient<IApplicationEventsService, ApplicationEventsService>()
                .AddTransient<IApplicationSettingsService, ApplicationSettingsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();


            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}