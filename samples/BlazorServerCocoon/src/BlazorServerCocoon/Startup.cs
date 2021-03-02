using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorServerCocoon.Data;
using BlazorServerCocoon.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReCode.Cocoon.Proxy.Authentication;
using ReCode.Cocoon.Proxy.Cookies;
using ReCode.Cocoon.Proxy.Proxy;

namespace BlazorServerCocoon
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
            services.AddCocoonSession();
            services.AddCocoonCookies();
            services.AddScoped<ShoppingCart>();

            services.AddAuthentication(CocoonAuthenticationDefaults.Scheme)
                .AddCocoon();

            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            
            var connectionString = Configuration.GetConnectionString("WingtipToys");
            services.AddDbContextPool<WingtipToysContext>(builder =>
            {
                builder.UseSqlServer(connectionString);
            });
            
            services.AddCocoonProxy(Configuration);
            
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCocoonProxyWithBlazorServer(typeof(Program));
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
    
    public class CustomAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        public CustomAuthenticationStateProvider(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);
    }
}
