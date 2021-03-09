using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ReverseProxy.Service.Proxy;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public static class FacadeProxyExtensions
    {
        public static IEndpointRouteBuilder MapFacadeProxy(this IEndpointRouteBuilder endpoints)
        {
            var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false
            });
            var transformer = new RedirectTransformer();
            var requestOptions = new RequestProxyOptions(TimeSpan.FromSeconds(100), null);
            var httpProxy = endpoints.ServiceProvider.GetRequiredService<IHttpProxy>();

            endpoints.Map("/{**catch-all}",
                async httpContext => { await httpProxy.ProxyAsync(httpContext, "http://localhost:24019/", httpClient, requestOptions, transformer); });

            return endpoints;
        }
    }
}