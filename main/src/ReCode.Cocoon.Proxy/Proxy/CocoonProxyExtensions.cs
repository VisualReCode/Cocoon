﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ReCode.Cocoon.Proxy.Proxy
{
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