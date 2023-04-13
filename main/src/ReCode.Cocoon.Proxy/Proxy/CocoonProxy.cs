using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Forwarder;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public class CocoonProxy
    {
        private readonly IHttpForwarder _httpForwarder;
        private readonly HashSet<string> _backendUrls;
        private readonly HttpMessageInvoker _httpClient;
        private readonly RedirectTransformer _transformer;
        private readonly ForwarderRequestConfig _requestConfig;
        private readonly string _destinationPrefix;
        private readonly HostHeaderOptions _hostHeaderOption;
        private readonly string _host;

        public CocoonProxy(IConfiguration configuration, ILogger<CocoonProxy> logger, IHttpForwarder httpForwarder, CocoonProxyOptions? proxyOptions)
        {
            _httpForwarder = httpForwarder;
            _destinationPrefix = configuration
                .GetValue<string>("Cocoon:Proxy:DestinationPrefix");

            if (string.IsNullOrEmpty(_destinationPrefix))
            {
                throw new InvalidOperationException("No DestinationPrefix");
            }

            if (!Uri.TryCreate(_destinationPrefix, UriKind.Absolute, out var destinationPrefixUri))
            {
                throw new InvalidOperationException("Invalid DestinationPrefix");
            }

            logger.LogInformation("Cocoon Proxy backend: {destinationPrefixUri}", destinationPrefixUri);

            // save the host
            _host = destinationPrefixUri.Host;
            logger.LogInformation("Cocoon Proxy backend: host is {host}", _host);

            _backendUrls = CocoonProxyExclusions.CreateExclusionSet(configuration);

            var socketsHttpHandler = new SocketsHttpHandler()
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.None,
                UseCookies = false
            };

            // Allow the client to validate the remote certificate, useful in development and self certificate scenarios
            if (proxyOptions?.RemoteCertificateValidationCallback != null)
            {
                socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = proxyOptions.RemoteCertificateValidationCallback;
            }

            _httpClient = new HttpMessageInvoker(socketsHttpHandler);
            _transformer = new RedirectTransformer(destinationPrefixUri);

            // save the host header option, or use default option
            _hostHeaderOption = proxyOptions?.HostHeaderOption ?? HostHeaderOptions.None;

            if (!TimeSpan.TryParse(configuration.GetValue<string>("Cocoon:Proxy:Timeout"), out var timeout))
            {
                timeout = TimeSpan.FromSeconds(30);
            }
            _requestConfig = new ForwarderRequestConfig()
            {
                ActivityTimeout = timeout
            };
        }

        public async Task ProxyAsync(HttpContext httpContext)
        {
            if (_backendUrls.Contains(httpContext.Request.Path))
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            // amend the HOST value in the request based on _hostHeaderOption
            switch (_hostHeaderOption)
            {
                case HostHeaderOptions.None:
                    // HostHeader unchanged
                    break;

                case HostHeaderOptions.SetDefault:
                    // HostHeader set to default
                    SetHost(httpContext.Request.Headers, default);
                    break;

                case HostHeaderOptions.UseHost:
                    // HostHeader set to _host
                    SetHost(httpContext.Request.Headers, _host);
                    break;

                default:
                    // unhandled options not supported
                    throw new NotSupportedException($"Unhandled HostHeaderOption {_hostHeaderOption}");
            }

            using var activity = ProxyActivitySource.StartActivity("Proxy");
            activity?.SetTag("path", httpContext.Request.Path.ToString());

            await _httpForwarder.SendAsync(httpContext, _destinationPrefix, _httpClient, _requestConfig, _transformer);
        }

        /// <summary>
        /// NET6 has useful 
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="host"></param>
        private static void SetHost(IHeaderDictionary headers, string? host)
        {
            headers["Host"] = host;
        }
    }
}