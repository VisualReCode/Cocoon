using System;
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
        public static IEndpointRouteBuilder MapCocoonProxy(this IEndpointRouteBuilder endpoints)
        {
            var destinationPrefix = endpoints.ServiceProvider.GetRequiredService<IConfiguration>().GetValue<string>("Cocoon:Proxy:DestinationPrefix");
            if (!Uri.TryCreate(destinationPrefix, UriKind.Absolute, out var destinationPrefixUri))
            {
                throw new InvalidOperationException("Invalid DestinationPrefix");
            }
            
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
                async httpContext => { await httpProxy.ProxyAsync(httpContext, destinationPrefix, httpClient, requestOptions, transformer); });

            return endpoints;
        }
    }
}