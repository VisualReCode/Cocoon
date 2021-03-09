using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.ReverseProxy.Service.Proxy;

namespace ReCode.Cocoon.Proxy.Proxy
{
    internal class RedirectTransformer : HttpTransformer
    {
        public override async Task TransformResponseAsync(HttpContext context, HttpResponseMessage response)
        {
            if (response.Headers.Location?.IsAbsoluteUri == true)
            {
                var relative = response.Headers.Location.PathAndQuery;
                response.Headers.Location = new Uri(relative, UriKind.Relative);
            }
            await base.TransformResponseAsync(context, response);
        }
    }
}