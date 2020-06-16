using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediaStat.Data;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication;
using MediaStat.Data.Services;

namespace MediaStat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<AppState>();
            services.AddScoped<EmployeeService>();
            services.AddScoped<AccountService>();
            services.AddScoped<LookupService>();
            services.AddScoped<LoginService>();
            services.AddBlazoredLocalStorage();
            services.AddOptions();
            services.AddAuthorizationCore();
            //services.AddScoped<AuthenticationService>(s =>
            //{
            //    return new AuthenticationService(URL);
            //});
            services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();
            services.AddDbContext<ApplicationDbContext>(options =>
                         options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));

            //services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
