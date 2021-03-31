using Microsoft.Extensions.Configuration;
using ReCode.Cocoon.Proxy.Proxy;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CocoonProxyServicesExtensions
    {
        public static IReverseProxyBuilder AddCocoonProxy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<CocoonProxy>();
            return services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));
        }
    }
}