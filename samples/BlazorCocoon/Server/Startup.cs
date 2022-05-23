using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorCocoon.Server.Data;
using BlazorCocoon.Server.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReCode.Cocoon.Proxy.Authentication;
using ReCode.Cocoon.Proxy.Blazor;

namespace BlazorCocoon.Server
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
            services.AddScoped<ShoppingCart>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddCocoonProxy(Configuration);
            
            var connectionString = Configuration.GetConnectionString("WingtipToys");
            services.AddDbContextPool<WingtipToysContext>(builder =>
            {
                builder.UseSqlServer(connectionString);
            });
            
            services.AddAuthentication(CocoonAuthenticationDefaults.Scheme)
                .AddCocoon();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapCocoonProxyWithBlazor(typeof(CocoonBlazorRouteTesterManual));
            });
        }
    }
}
