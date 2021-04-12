using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ReverseProxy.Service.Proxy;
using ReCode.Cocoon.Proxy.Proxy;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class CocoonProxyServicesExtensions
    {
        public static IReverseProxyBuilder AddCocoonProxy(this IServiceCollection services, IConfiguration configuration)
        {
            return AddCocoonProxy(services, configuration, null);
        }

        public static IReverseProxyBuilder AddCocoonProxy(this IServiceCollection services, IConfiguration configuration, CocoonProxyOptions? cocoonProxyOptions)
        {
            services.AddSingleton<CocoonProxy>(provider => new CocoonProxy(
                configuration, 
                provider.GetService<ILogger<CocoonProxy>>(), 
                provider.GetService<IHttpProxy>(), cocoonProxyOptions));

            return ReverseProxyBuilder(services, configuration);
        }

        private static IReverseProxyBuilder ReverseProxyBuilder(IServiceCollection services, IConfiguration configuration)
        {
            return services.AddReverseProxy().LoadFromConfig(configuration.GetSection("ReverseProxy"));
        }
    }
}