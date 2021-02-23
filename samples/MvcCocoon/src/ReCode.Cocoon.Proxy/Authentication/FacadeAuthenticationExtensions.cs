using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Authentication
{
    public static class FacadeAuthenticationExtensions
    {
        public static AuthenticationBuilder AddFacade(this AuthenticationBuilder builder)
        {
            builder.Services.AddOptions<FacadeAuthenticationOptions>()
                .BindConfiguration("FacadeAuthentication")
                .Validate(o => Uri.TryCreate(o.BackendUrl, UriKind.Absolute, out _),
                    "FacadeAuthentication:BackendUrl is not set.");
            
            builder.Services.AddHttpClient<AuthenticationClient>((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<FacadeAuthenticationOptions>>();
                client.BaseAddress = new Uri(options.CurrentValue.BackendUrl);
            });

            builder.AddScheme<FacadeAuthenticationOptions, FacadeAuthenticationHandler>("Facade", null);

            return builder;
        }
    }
}