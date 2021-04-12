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
        public static IEndpointRouteBuilder MapCocoonProxyWithBlazor(this IEndpointRouteBuilder endpoints, Type type)
        {
            Func<string, bool> blazorTestFunc;
            if (type.Name == "CocoonBlazorRouteTester")
            {
                var instance = Activator.CreateInstance(type);
                var method = type.GetMethod("IsMatch");
                blazorTestFunc = (Func<string, bool>) method!.CreateDelegate(typeof(Func<string, bool>), instance);
            }
            else
            {
                var blazorPaths = BlazorRouteDiscovery.FindRoutes(type);
                var blazorRoutes = new BlazorRoutes(blazorPaths);
                var method = typeof(BlazorRoutes).GetMethod("Contains");
                blazorTestFunc = (Func<string, bool>) method!.CreateDelegate(typeof(Func<string, bool>), blazorRoutes);
            }

            return MapCocoonProxyWithBlazor(endpoints, blazorTestFunc);
        }

        public static IEndpointRouteBuilder MapCocoonProxyWithBlazor(this IEndpointRouteBuilder endpoints, Func<string, bool> blazorRouteTest)
        {
            var cocoonProxy = endpoints.ServiceProvider.GetRequiredService<CocoonProxy>();
            
            var app = endpoints.CreateApplicationBuilder();

            app.Use(async (httpContext, next) =>
            {
                if (blazorRouteTest(httpContext.Request.Path))
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