using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Authentication
{
    public static class CocoonAuthenticationExtensions
    {
        public static AuthenticationBuilder AddCocoon(this AuthenticationBuilder builder)
        {
            builder.Services.AddOptions<CocoonAuthenticationOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection("Cocoon.Authentication").Bind(options);
                })
                .Validate(o => Uri.TryCreate(o.BackendApiUrl, UriKind.Absolute, out _),
                    "Invalid BackendApiUrl");
            
            builder.Services.AddHttpClient<CocoonAuthenticationClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<CocoonAuthenticationOptions>>();
                client.BaseAddress = new Uri(options.CurrentValue.BackendApiUrl);
            });
            
            builder.AddScheme<CocoonAuthenticationOptions, CocoonAuthenticationHandler>(CocoonAuthenticationDefaults.Scheme, null);

            return builder;
        }
    }
}