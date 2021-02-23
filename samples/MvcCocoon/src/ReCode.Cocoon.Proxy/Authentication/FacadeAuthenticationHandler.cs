using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Authentication
{
    public class FacadeAuthenticationHandler : AuthenticationHandler<FacadeAuthenticationOptions>
    {
        private readonly AuthenticationClient _client;

        public FacadeAuthenticationHandler(AuthenticationClient client, IOptionsMonitor<FacadeAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _client = client;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await _client.AuthenticateAsync(Request);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var relativeUri = Request.GetEncodedPathAndQuery();
            var escaped = Uri.EscapeDataString(relativeUri);
            Response.StatusCode = 302;
            Response.Headers["Location"] = $"/Account/Login?ReturnUrl={escaped}";
            return Task.CompletedTask;
        }
    }
}