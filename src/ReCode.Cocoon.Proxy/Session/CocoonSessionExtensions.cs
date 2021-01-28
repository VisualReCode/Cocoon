using System;
using Microsoft.Extensions.Options;
using ReCode.Cocoon.Proxy.Session;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CocoonSessionExtensions
    {
        public static IServiceCollection AddCocoonSession(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddOptions<CocoonSessionOptions>()
                .BindConfiguration("Cocoon:Session")
                .Validate(o => Uri.TryCreate(o.BackendApiUrl, UriKind.Absolute, out _),
                    "Invalid BackendApiUrl");
            services.AddHttpClient<CocoonSessionClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<CocoonSessionOptions>>();
                client.BaseAddress = new Uri(options.CurrentValue.BackendApiUrl);
            });
            services.AddScoped<CocoonSession>();
            return services;
        }
    }
}