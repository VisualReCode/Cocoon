using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public static class CocoonProxyExclusions
    {
        public static HashSet<string> CreateExclusionSet(IConfiguration configuration)
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