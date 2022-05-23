using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ReCode.Cocoon.Proxy.Proxy;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing
{
    [PublicAPI]
    public static class CocoonProxyExtensions
    {
        public static IEndpointRouteBuilder MapCocoonProxy(this IEndpointRouteBuilder endpoints)
        {
            var cocoonProxy = endpoints.ServiceProvider.GetRequiredService<CocoonProxy>();

            endpoints.Map("/{**catch-all}", cocoonProxy.ProxyAsync);

            return endpoints;
        }
    }
}