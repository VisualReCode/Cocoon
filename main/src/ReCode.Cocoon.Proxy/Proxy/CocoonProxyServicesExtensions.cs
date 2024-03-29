using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReCode.Cocoon.Proxy.Proxy;
using Yarp.ReverseProxy.Forwarder;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    [PublicAPI]
    public static class CocoonProxyServicesExtensions
    {
        public static IReverseProxyBuilder AddCocoonProxy(this IServiceCollection services, IConfiguration configuration)
        {
            return AddCocoonProxy(services, configuration, null);
        }

        public static IReverseProxyBuilder AddCocoonProxy(this IServiceCollection services, IConfiguration configuration, CocoonProxyOptions? cocoonProxyOptions)
        {
            services.AddSingleton(provider => new CocoonProxy(
                configuration, 
                provider.GetRequiredService<ILogger<CocoonProxy>>(), 
                provider.GetRequiredService<IHttpForwarder>(), cocoonProxyOptions));

            return ReverseProxyBuilder(services, configuration);
        }

        private static IReverseProxyBuilder ReverseProxyBuilder(IServiceCollection services, IConfiguration configuration)
        {
            return services.AddReverseProxy().LoadFromConfig(configuration.GetSection("ReverseProxy"));
        }
    }
}