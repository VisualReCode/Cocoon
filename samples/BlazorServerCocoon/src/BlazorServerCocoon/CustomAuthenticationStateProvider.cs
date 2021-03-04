using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;

namespace BlazorServerCocoon
{
    public class CustomAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        public CustomAuthenticationStateProvider(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);
    }
}