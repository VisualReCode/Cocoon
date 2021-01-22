using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Session
{
    public class CocoonSession
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Uri _uri;
        private readonly object _mutex = new();
        private Dictionary<string, object> _cache;

        public CocoonSession(IHttpContextAccessor contextAccessor, IOptionsMonitor<CocoonSessionOptions> options)
        {
            _contextAccessor = contextAccessor;
            _uri = new Uri(options.CurrentValue.BackendApiUrl);
        }

        public ValueTask<T> GetAsync<T>(string key)
        {
            lock (_mutex)
            {
                if (_cache is null)
                {
                    _cache = new Dictionary<string, object>();
                }
                else
                {
                    if (_cache.TryGetValue(key, out var value))
                    {
                        return new ValueTask<T>((T) value);
                    }
                }
            }
            
            return new ValueTask<T>(Get<T>(key));
        }

        private async Task<T> Get<T>(string key)
        {
            var context = _contextAccessor.HttpContext;
            if (context is null) throw new InvalidOperationException("No context");
            var client = new HttpClient()
            {
                BaseAddress = _uri
            };

            var request = new HttpRequestMessage(HttpMethod.Get, $"?key={key}");
            if (context.Request.Cookies.TryGetValue("ASP.NET_SessionId", out var sessionId))
            {
                request.Headers.Add("Cookie", $"ASP.NET_SessionId={sessionId}");
                // cookieContainer.Add(new Cookie("ASP.NET_SessionId", sessionId));
            }

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var bytes = await response.Content.ReadAsByteArrayAsync();

            var value = SessionValueDeserializer.Deserialize<T>(bytes);
            
            CacheValue(key, value);

            return (T)value;
        }

        private void CacheValue(string key, object value)
        {
            lock (_mutex)
            {
                _cache[key] = value;
            }
        }
    }
}