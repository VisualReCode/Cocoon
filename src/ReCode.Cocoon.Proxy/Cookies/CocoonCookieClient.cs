using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using ReCode.Cocoon.Proxy.Session;

namespace ReCode.Cocoon.Proxy.Cookies
{
    public class CocoonCookieClient
    {
        private readonly HttpClient _client;

        public CocoonCookieClient(HttpClient client, IOptionsMonitor<CocoonSessionOptions> options)
        {
            _client = client;
        }

        public async Task<string?> GetAsync(string key, HttpRequest request)
        {
            var message = CreateMessage(request, HttpMethod.Get, $"?key={key}");

            var response = await _client.SendAsync(message);
            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetAsync(string key, string value, HttpContext context)
        {
            var uri = $"?key={key}";
            
            var message = CreateMessage(context.Request, HttpMethod.Put, uri);

            message.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(value));

            var response = await _client.SendAsync(message);

            if (response.Headers.TryGetValues(HeaderNames.SetCookie, out var values))
            {
                var newValue = values.FirstOrDefault(v => v.StartsWith($"{key}="));
                if (newValue is null) return;
                context.Response.Headers.Add(HeaderNames.SetCookie, newValue);
            }
        }

        private static HttpRequestMessage CreateMessage(HttpRequest request, HttpMethod httpMethod, string? requestUri)
        {
            var message = new HttpRequestMessage(httpMethod, requestUri);
            if (request.Headers.TryGetValue("Cookie", out var cookieHeaders))
            {
                foreach (var cookieHeader in cookieHeaders)
                {
                    message.Headers.Add("Cookie", cookieHeader);
                }
            }
            return message;
        }
    }
}