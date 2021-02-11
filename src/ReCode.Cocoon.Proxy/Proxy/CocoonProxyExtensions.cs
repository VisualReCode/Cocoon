using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ReverseProxy.Service.Proxy;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public static class CocoonProxyExtensions
    {
        private static ActivitySource Source = new("ReCode.Cocoon.Proxy");

        public static IReverseProxyBuilder AddCocoonProxy(this IServiceCollection services, IConfiguration configuration) =>
            services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));

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
}