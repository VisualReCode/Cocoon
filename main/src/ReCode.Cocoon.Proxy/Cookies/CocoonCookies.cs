using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ReCode.Cocoon.Proxy.Cookies
{
    public class CocoonCookies : ICocoonCookies
    {
        private readonly CocoonCookieClient _client;
        private readonly IHttpContextAccessor _contextAccessor;

        public CocoonCookies(CocoonCookieClient client, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _contextAccessor = contextAccessor;
        }

        public ValueTask<string?> GetAsync(string key)
        {
            var context = _contextAccessor.HttpContext;
            if (context is null) throw new InvalidOperationException("No context");
            return new ValueTask<string?>(_client.GetAsync(key, context.Request));
        }

        public async Task SetAsync(string key, string value)
        {
            var context = _contextAccessor.HttpContext;
            if (context is null) throw new InvalidOperationException("No context");

            await _client.SetAsync(key, value, context);
        }
    }
}