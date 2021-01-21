using ReCode.Cocoon.Proxy.Session;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class FacadeSessionExtensions
    {
        public static IServiceCollection AddFacadeSession(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<FacadeSession>();
            return services;
        }
    }
}