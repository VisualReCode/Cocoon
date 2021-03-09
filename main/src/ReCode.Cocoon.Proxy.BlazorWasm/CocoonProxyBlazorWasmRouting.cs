using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ReCode.Cocoon.Proxy.Proxy;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Routing
{
    public static class CocoonProxyBlazorWasmRouting
    {
        public static IEndpointRouteBuilder MapCocoonProxyWithBlazor(this IEndpointRouteBuilder endpoints, Type programType) =>
            MapCocoonProxyWithBlazor(endpoints, BlazorRouteDiscovery.FindRoutes(programType));

        public static IEndpointRouteBuilder MapCocoonProxyWithBlazor(this IEndpointRouteBuilder endpoints, IEnumerable<string> blazorPaths)
        {
            var cocoonProxy = endpoints.ServiceProvider.GetRequiredService<CocoonProxy>();
            
            var blazorRoutes = new BlazorRoutes(blazorPaths);
            
            var app = endpoints.CreateApplicationBuilder();

            app.Use(async (httpContext, next) =>
            {
                if (blazorRoutes.Contains(httpContext.Request.Path))
                {
                    httpContext.Request.Path = "/index.html";

                    // Set endpoint to null so the static files middleware will handle the request.
                    httpContext.SetEndpoint(null);

                    await next();
                    return;
                }

                await cocoonProxy.ProxyAsync(httpContext);
            });
            
            app.UseStaticFiles();

            var func = app.Build();
            
            endpoints.MapFallback("/{**catch-all}", func);

            return endpoints;
        }
    }
}