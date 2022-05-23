using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorServerCocoon.Data;
using BlazorServerCocoon.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
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

            // This has to go before app.UseRouting();
            app.UseExplicitBlazorRoutes(typeof(Program));

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapCocoonProxy();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
