using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace ReCode.Cocoon.Proxy.Session
{
    [PublicAPI]
    public class CocoonSession : IAsyncDisposable
    {
        private readonly CocoonSessionClient _client;
        private readonly HttpContext? _context;
        private readonly object _mutex = new();
        private Dictionary<string, byte[]?>? _original;
        private Dictionary<string, object?>? _cache;
        private int _disposed;

        public CocoonSession(CocoonSessionClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _context = contextAccessor.HttpContext;
        }

        public ValueTask<T?> GetAsync<T>(string key)
        {
            EnsureCache();
            lock (_mutex)
            {
                if (_cache!.TryGetValue(key, out var value))
                {
                    return new ValueTask<T?>(value is null ? default : (T)value);
                }
            }
            
            return new ValueTask<T?>(Get<T>(key));
        }

        public void Set<T>(string key, T? value)
        {
            EnsureCache();
            CacheValue(key, value);
        }

        private async Task<T?> Get<T>(string key)
        {
            if (_context is null) return default;
            
            using var activity = ProxyActivitySource.StartActivity("GetSession");
            activity?.AddTag("key", key);
            
            var bytes = await _client.GetAsync(key, _context.Request);
            if (bytes is null) return default;
            
            var value = SessionValueDeserializer.Deserialize<T>(bytes);
            
            CacheValue(key, bytes, value);

            return value is null ? default : (T)value;
        }

        private void CacheValue(string key, byte[]? bytes, object? value)
        {
            lock (_mutex)
            {
                _original![key] = bytes;
                _cache![key] = value;
            }
        }

        private void CacheValue(string key, object? value)
        {
            lock (_mutex)
            {
                _cache![key] = value;
            }
        }

        private void EnsureCache()
        {
            lock (_mutex)
            {
                _cache ??= new Dictionary<string, object?>();
                _original ??= new Dictionary<string, byte[]?>();
            }
        }

        public ValueTask DisposeAsync()
        {
            if (_cache is null) return default;
            
            if (Interlocked.Increment(ref _disposed) > 1) return default;
            
            using var activity = ProxyActivitySource.StartActivity("SaveSession");

            List<Task>? tasks = null;
            
            foreach (var (key, value) in _cache)
            {
                var type = value?.GetType() ?? typeof(object);
                
                var bytes = ValueSerializer.Serialize(value);
                
                if (_original!.TryGetValue(key, out var original))
                {
                    if (!bytes.AsSpan().SequenceEqual(original))
                    {
                        SendValue(key, bytes, type, ref tasks);
                    }
                }
                else
                {
                    SendValue(key, bytes, type, ref tasks);
                }
            }

            if (tasks is null)
            {
                activity?.AddTag("count", Numbers[0]);
                return new ValueTask();
            }

            object count = tasks.Count < 10
                ? Numbers[tasks.Count]
                : tasks.Count;

            activity?.AddTag("count", count);
            return new ValueTask(Task.WhenAll(tasks));
        }

        private void SendValue(string key, byte[] bytes, Type type, ref List<Task>? tasks)
        {
            if (_context is null) return;
            
            var task = _client.SetAsync(key, bytes, type, _context.Request);
            tasks ??= new();
            tasks.Add(task);
        }

        private static readonly object[] Numbers = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    }
}