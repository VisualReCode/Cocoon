using Microsoft.AspNetCore.Components.Authorization;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class BlazorWasmAuthRegistrations
    {
        public static IServiceCollection AddCocoonAuthentication(this IServiceCollection services)
        {
            services.AddScoped<AuthenticationStateProvider, CocoonAuthStateProvider>();
            services.AddAuthorizationCore();
            return services;
        }
    }
}