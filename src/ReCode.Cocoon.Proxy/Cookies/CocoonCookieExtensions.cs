using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ReCode.Cocoon.Proxy.Session;

namespace ReCode.Cocoon.Proxy.Cookies
{
    public static class CocoonCookieExtensions
    {
        public static IServiceCollection AddCocoonCookies(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddOptions<CocoonCookieOptions>()
                .BindConfiguration("Cocoon:Cookies")
                .Validate(o => Uri.TryCreate(o.BackendApiUrl, UriKind.Absolute, out _),
                    "Invalid BackendApiUrl");
            services.AddHttpClient<CocoonCookieClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<CocoonCookieOptions>>();
                client.BaseAddress = new Uri(options.CurrentValue.BackendApiUrl);
            });
            services.AddScoped<ICocoonCookies, CocoonCookies>();
            return services;
        }
    }
}