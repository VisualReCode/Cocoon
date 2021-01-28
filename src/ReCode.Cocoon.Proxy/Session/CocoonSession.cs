using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Session
{
    public class CocoonSession
    {
        private readonly CocoonSessionClient _client;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly object _mutex = new();
        private Dictionary<string, object> _cache;

        public CocoonSession(CocoonSessionClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor;
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

        public async Task SetAsync<T>(string key, T value)
        {
            lock (_mutex)
            {
                if (_cache is null)
                {
                    _cache = new Dictionary<string, object>();
                }
            }

            object obj = value;
            
            CacheValue(key, obj);

            var context = _contextAccessor.HttpContext;
            if (context is null) throw new InvalidOperationException("No context");
            await _client.SetAsync(key, obj, typeof(T), context.Request);
        }

        private async Task<T> Get<T>(string key)
        {
            var context = _contextAccessor.HttpContext;
            if (context is null) throw new InvalidOperationException("No context");
            var value = await _client.GetAsync<T>(key, context.Request);
            
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