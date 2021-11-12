using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Forwarder;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public class RedirectTransformer : HttpTransformer
    {
        private readonly Uri _destinationPrefix;

        public RedirectTransformer(Uri destinationPrefix)
        {
            _destinationPrefix = destinationPrefix;
        }

        public override ValueTask TransformResponseTrailersAsync(HttpContext context, HttpResponseMessage response)
        {
            var location = response.Headers.Location;
            
            if (location?.IsAbsoluteUri == true && _destinationPrefix.IsBaseOf(location))
            {
                var relative = location.PathAndQuery;
                response.Headers.Location = new Uri(relative, UriKind.Relative);
            }
            return base.TransformResponseTrailersAsync(context, response);
        }
    }
}