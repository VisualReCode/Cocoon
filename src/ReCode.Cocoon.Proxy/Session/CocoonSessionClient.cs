using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Session
{
    public class CocoonSessionClient
    {
        private readonly HttpClient _client;

        public CocoonSessionClient(HttpClient client, IOptionsMonitor<CocoonSessionOptions> options)
        {
            _client = client;
        }

        public async Task<byte[]> GetAsync(string key, HttpRequest request)
        {
            var message = CreateMessage(key, request, HttpMethod.Get, $"?key={key}");

            using var response = await _client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task SetAsync(string key, object value, Type type, HttpRequest request)
        {
            var uri = $"?key={key}&type={type.FullName}";
            
            var message = CreateMessage(key, request, HttpMethod.Put, uri);

            var bytes = ValueSerializer.Serialize(value);

            MemoryPool<byte>.Shared.Rent(100);

            message.Content = new ByteArrayContent(bytes);

            await _client.SendAsync(message);
        }

        public async Task SetAsync(string key, byte[] bytes, Type type, HttpRequest request)
        {
            var uri = $"?key={key}&type={type.AssemblyQualifiedName}";
            
            var message = CreateMessage(key, request, HttpMethod.Put, uri);

            message.Content = new ByteArrayContent(bytes);

            await _client.SendAsync(message);
        }

        private static HttpRequestMessage CreateMessage(string key, HttpRequest request, HttpMethod httpMethod, string? requestUri)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri);
            if (request.Cookies.TryGetValue("ASP.NET_SessionId", out var sessionId))
            {
                message.Headers.Add("Cookie", $"ASP.NET_SessionId={sessionId}");
            }

            return message;
        }
    }
}