﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Yarp.ReverseProxy.Service.Proxy;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public class CocoonProxy
    {
        private static readonly ActivitySource Source = new("ReCode.Cocoon.Proxy");
        private readonly IHttpProxy _httpProxy;
        private readonly HashSet<string> _backendUrls;
        private readonly HttpMessageInvoker _httpClient;
        private readonly RedirectTransformer _transformer;
        private readonly RequestProxyOptions _requestOptions;
        private readonly string _destinationPrefix;

        public CocoonProxy(IConfiguration configuration, ILogger<CocoonProxy> logger, IHttpProxy httpProxy, CocoonProxyOptions? proxyOptions)
        {
            _httpProxy = httpProxy;
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

            logger.LogInformation($"Cocoon Proxy backend: {destinationPrefixUri}");

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

            if (!TimeSpan.TryParse(configuration.GetValue<string>("Cocoon:Proxy:Timeout"), out var timeout))
            {
                timeout = TimeSpan.FromSeconds(30);
            }
            _requestOptions = new RequestProxyOptions
            {
                Timeout = timeout
            };
        }

        public async Task ProxyAsync(HttpContext httpContext)
        {
            if (_backendUrls.Contains(httpContext.Request.Path))
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            using var activity = Source.StartActivity("Proxy");
            activity?.SetTag("path", httpContext.Request.Path.ToString());

            await _httpProxy.ProxyAsync(httpContext, _destinationPrefix, _httpClient, _requestOptions, _transformer);
        }
    }
}