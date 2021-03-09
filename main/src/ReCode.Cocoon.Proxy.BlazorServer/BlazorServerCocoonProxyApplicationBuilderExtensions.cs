using System;
using System.Collections;
using System.Collections.Generic;
using ReCode.Cocoon.Proxy.Proxy;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class BlazorServerCocoonProxyApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExplicitBlazorRoutes(this IApplicationBuilder app, Type programType) =>
            app.UseExplicitBlazorRoutes(BlazorRouteDiscovery.FindRoutes(programType));
        
        public static IApplicationBuilder UseExplicitBlazorRoutes(this IApplicationBuilder app, IEnumerable<string> blazorRouteTemplates)
        {
            var routes = new BlazorRoutes(blazorRouteTemplates);
            app.Use((context, next) =>
            {
                if (routes.Contains(context.Request.Path))
                {
                    context.Request.Path = "/";
                }

                return next();
            });
            return app;
        }
    }
}