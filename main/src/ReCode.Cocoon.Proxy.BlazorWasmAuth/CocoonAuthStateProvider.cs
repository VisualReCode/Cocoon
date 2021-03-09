using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components.Authorization
{
    public class CocoonAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;

        public CocoonAuthStateProvider(HttpClient http)
        {
            _http = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var cocoonPrincipal =
                await _http.GetFromJsonAsync<CocoonPrincipal>("/_cocoon/auth");

            if (cocoonPrincipal.IsAuthenticated)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, cocoonPrincipal.Name),
                }, "Fake authentication type");

                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            else
            {
                var user = new ClaimsPrincipal();
                return new AuthenticationState(user);
            }
            
        }
    }
}
