using System;
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

        public async Task<object> GetAsync<T>(string key, HttpRequest request)
        {
            var message = CreateMessage(key, request, HttpMethod.Get, $"?key={key}");

            var response = await _client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var bytes = await response.Content.ReadAsByteArrayAsync();

            var value = SessionValueDeserializer.Deserialize<T>(bytes);
            
            return value;
        }

        public async Task SetAsync(string key, object value, Type type, HttpRequest request)
        {
            var uri = $"?key={key}&type={type.FullName}";
            
            var message = CreateMessage(key, request, HttpMethod.Put, uri);

            var bytes = ValueSerializer.Serialize(value);

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