using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ReverseProxy.Service.Proxy;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public static class CocoonProxyExtensions
    {
        private static readonly ActivitySource Source = new("ReCode.Cocoon.Proxy");

        public static IReverseProxyBuilder AddCocoonProxy(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));
        }

        public static IEndpointRouteBuilder MapCocoonProxy(this IEndpointRouteBuilder endpoints)
        {
            var configuration = endpoints.ServiceProvider
                .GetRequiredService<IConfiguration>();

            var destinationPrefix = configuration
                .GetValue<string>("Cocoon:Proxy:DestinationPrefix");

            if (!Uri.TryCreate(destinationPrefix, UriKind.Absolute, out var destinationPrefixUri))
            {
                throw new InvalidOperationException("Invalid DestinationPrefix");
            }

            var backendUrls = CreateExclusionSet(configuration);

            var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false
            });

            var transformer = new RedirectTransformer(destinationPrefixUri);
            var requestOptions = new RequestProxyOptions(TimeSpan.FromSeconds(100), null);
            var httpProxy = endpoints.ServiceProvider.GetRequiredService<IHttpProxy>();

            endpoints.Map("/{**catch-all}",
                async httpContext =>
                {
                    if (backendUrls.Contains(httpContext.Request.Path))
                    {
                        httpContext.Response.StatusCode = 404;
                        return;
                    }
            
                    using var activity = Source.StartActivity("Proxy");
                    activity?.SetTag("path", httpContext.Request.Path.ToString());
            
                    await httpProxy.ProxyAsync(httpContext, destinationPrefix, httpClient, requestOptions, transformer);
                });

            return endpoints;
        }

        public static IApplicationBuilder UseExplicitBlazorRoutes(this IApplicationBuilder app, Type programType)
        {
            var routes = new BlazorRoutes(BlazorRouteDiscovery.FindRoutes(programType));
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

        public static IEndpointRouteBuilder MapCocoonProxyWithBlazor(this IEndpointRouteBuilder endpoints, Type programType) =>
            MapCocoonProxyWithBlazor(endpoints, BlazorRouteDiscovery.FindRoutes(programType));

        public static IEndpointRouteBuilder MapCocoonProxyWithBlazor(this IEndpointRouteBuilder endpoints, IEnumerable<string> blazorPaths)
        {
            var blazorRoutes = new BlazorRoutes(blazorPaths);
            
            var configuration = endpoints.ServiceProvider
                .GetRequiredService<IConfiguration>();

            var destinationPrefix = configuration
                .GetValue<string>("Cocoon:Proxy:DestinationPrefix");

            if (!Uri.TryCreate(destinationPrefix, UriKind.Absolute, out var destinationPrefixUri))
            {
                throw new InvalidOperationException("Invalid DestinationPrefix");
            }

            var backendUrls = CreateExclusionSet(configuration);

            var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false
            });

            var transformer = new RedirectTransformer(destinationPrefixUri);
            var requestOptions = new RequestProxyOptions(TimeSpan.FromSeconds(100), null);
            var httpProxy = endpoints.ServiceProvider.GetRequiredService<IHttpProxy>();

            var app = endpoints.CreateApplicationBuilder();

            app.Use(async (httpContext, next) =>
            {
                if (backendUrls.Contains(httpContext.Request.Path))
                {
                    httpContext.Response.StatusCode = 404;
                    return;
                }

                if (blazorRoutes.Contains(httpContext.Request.Path))
                {
                    httpContext.Request.Path = "/index.html";

                    // Set endpoint to null so the static files middleware will handle the request.
                    httpContext.SetEndpoint(null);

                    await next();
                    return;
                }

                using var activity = Source.StartActivity("Proxy");
                activity?.SetTag("path", httpContext.Request.Path.ToString());

                await httpProxy.ProxyAsync(httpContext, destinationPrefix, httpClient, requestOptions, transformer);
            });
            
            app.UseStaticFiles();

            var func = app.Build();
            
            endpoints.MapFallback("/{**catch-all}", func);

            return endpoints;
        }
        
        public static IEndpointRouteBuilder MapCocoonProxyWithBlazorServer(this IEndpointRouteBuilder endpoints, Type programType) =>
            MapCocoonProxyWithBlazorServer(endpoints, BlazorRouteDiscovery.FindRoutes(programType));

        public static IEndpointRouteBuilder MapCocoonProxyWithBlazorServer(this IEndpointRouteBuilder endpoints, IEnumerable<string> blazorPaths)
        {
            var configuration = endpoints.ServiceProvider
                .GetRequiredService<IConfiguration>();

            var destinationPrefix = configuration
                .GetValue<string>("Cocoon:Proxy:DestinationPrefix");

            if (!Uri.TryCreate(destinationPrefix, UriKind.Absolute, out var destinationPrefixUri))
            {
                throw new InvalidOperationException("Invalid DestinationPrefix");
            }

            var backendUrls = CreateExclusionSet(configuration);

            var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false
            });

            var transformer = new RedirectTransformer(destinationPrefixUri);
            var requestOptions = new RequestProxyOptions(TimeSpan.FromSeconds(100), null);
            var httpProxy = endpoints.ServiceProvider.GetRequiredService<IHttpProxy>();
            
            endpoints.Map("/{**catch-all}",
                async httpContext =>
                {
                    if (backendUrls.Contains(httpContext.Request.Path))
                    {
                        httpContext.Response.StatusCode = 404;
                        return;
                    }

                    using var activity = Source.StartActivity("Proxy");
                    activity?.SetTag("path", httpContext.Request.Path.ToString());
            
                    await httpProxy.ProxyAsync(httpContext, destinationPrefix, httpClient, requestOptions, transformer);
                });

            return endpoints;
        }


        private static HashSet<string> CreateExclusionSet(IConfiguration configuration)
        {
            var sessionApiUrl = configuration
                .GetValue<string>("Cocoon:Session:BackendApiUrl");
            var authApiUrl = configuration
                .GetValue<string>("Cocoon:Authentication:BackendApiUrl");
            var cookieApiUrl = configuration
                .GetValue<string>("Cocoon:Cookies:BackendApiUrl");

            return CreateExclusionSet(sessionApiUrl, authApiUrl, cookieApiUrl);
        }

        private static HashSet<string> CreateExclusionSet(params string[] urls)
        {
            var backendUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var url in urls)
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
                {
                    backendUrls.Add(uri.AbsolutePath);
                }
            }

            return backendUrls;
        }
    }
    
    public class BlazorRouteTransformer : DynamicRouteValueTransformer
    {
        public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
        {
            return new(values);
        }
    }
}