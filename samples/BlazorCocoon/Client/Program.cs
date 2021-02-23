using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorCocoon.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddAuthorizationCore();
            
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
    
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;

        public CustomAuthStateProvider(HttpClient http)
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
    
    public class CocoonPrincipal
    {
        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
