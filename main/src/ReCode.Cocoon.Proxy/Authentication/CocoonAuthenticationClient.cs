﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Authentication
{
    public class CocoonAuthenticationClient
    {
        private static readonly ActivitySource Source = new ("ReCode.Cocoon.Proxy");
        
        private readonly HttpClient _httpClient;
        private readonly IOptionsMonitor<CocoonAuthenticationOptions> _options;
        private readonly ILogger<CocoonAuthenticationClient> _logger;

        public CocoonAuthenticationClient(HttpClient httpClient, IOptionsMonitor<CocoonAuthenticationOptions> options, ILogger<CocoonAuthenticationClient> logger)
        {
            _httpClient = httpClient;
            _options = options;
            _logger = logger;
        }

        public async Task<AuthenticateResult> AuthenticateAsync(HttpRequest request)
        {
            using var activity = Source.StartActivity("Authenticate");
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "");

            CopyHeaders(request, requestMessage);

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);

                activity?.SetTag("ResponseStatusCode", (int)response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    var claimsPrincipal = await DeserializePrincipal(stream);
                    var ticket = new AuthenticationTicket(claimsPrincipal, CocoonAuthenticationDefaults.Scheme);
                    return AuthenticateResult.Success(ticket);
                }

                return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return AuthenticateResult.Fail(ex);
            }
        }

        private void CopyHeaders(HttpRequest request, HttpRequestMessage requestMessage)
        {
            bool fromConfig = false;
            if (_options.CurrentValue.Cookies is {Length: > 0})
            {
                fromConfig = true;
                CopyOptionsCookies(request, requestMessage);
            }

            if (_options.CurrentValue.Headers is {Length: > 0})
            {
                fromConfig = true;
                CopyOptionsHeaders(request, requestMessage);
            }

            if (!fromConfig)
            {
                CopyDefaultHeaders(request, requestMessage);
            }
        }

        private static void CopyDefaultHeaders(HttpRequest request, HttpRequestMessage requestMessage)
        {
            foreach (var header in request.Headers)
            {
                if (header.Key.Equals("Cookie", StringComparison.OrdinalIgnoreCase)
                    || header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                {
                    requestMessage.Headers.Add(header.Key, header.Value.AsEnumerable());
                }
            }
        }

        private void CopyOptionsHeaders(HttpRequest request, HttpRequestMessage requestMessage)
        {
            foreach (var headerName in _options.CurrentValue.Headers)
            {
                if (request.Headers.TryGetValue(headerName, out var values))
                {
                    if (values.Count == 1)
                    {
                        requestMessage.Headers.Add(headerName, $"{headerName}={(string) values}");
                    }
                    else
                    {
                        foreach (var value in values)
                        {
                            requestMessage.Headers.Add(headerName, $"{headerName}={value}");
                        }
                    }
                }
            }
        }

        private void CopyOptionsCookies(HttpRequest request, HttpRequestMessage requestMessage)
        {
            foreach (var cookieName in _options.CurrentValue.Cookies)
            {
                if (request.Cookies.TryGetValue(cookieName, out var value))
                {
                    requestMessage.Headers.Add("Cookie", $"{cookieName}={value}");
                }
            }
        }

        private static async Task<ClaimsPrincipal> DeserializePrincipal(Stream stream)
        {
            var messagePrincipal =
                await MessagePackSerializer.DeserializeAsync<MessagePrincipal>(stream);
            return messagePrincipal.ToClaimsPrincipal();
        }
    }
}