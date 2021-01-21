using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Http;

namespace ReCode.Cocoon.Proxy.Session
{
    public class FacadeSession
    {
        private const string BaseAddress = "http://localhost:24019/facadesession";
        private readonly IHttpContextAccessor _contextAccessor;

        public FacadeSession(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var context = _contextAccessor.HttpContext;
            if (context is null) throw new InvalidOperationException("No context");
            var client = new HttpClient()
            {
                BaseAddress = new Uri(BaseAddress)
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

            var value = Deserialize<T>(bytes);

            return value;
        }

        private static T Deserialize<T>(byte[] bytes)
        {
            if (Deserializers.TryGetValue(typeof(T), out var deserializer))
            {
                return (T) deserializer(bytes);
            }
            
            return  MessagePackSerializer.Deserialize<T>(bytes, TypelessContractlessStandardResolver.Options);
        }

        private static readonly Dictionary<Type, Func<byte[], object>> Deserializers = new()
        {
            [typeof(string)] = bytes => Encoding.UTF8.GetString(bytes),
            [typeof(int)] = bytes => BitConverter.ToInt32(bytes),
            [typeof(long)] = bytes => BitConverter.ToInt64(bytes),
        };
    }
}