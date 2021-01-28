using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Authentication
{
    public class CocoonAuthenticationHandler : AuthenticationHandler<CocoonAuthenticationOptions>
    {
        private readonly CocoonAuthenticationClient _client;

        public CocoonAuthenticationHandler(CocoonAuthenticationClient client, IOptionsMonitor<CocoonAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
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
            if (string.IsNullOrEmpty(Options.LoginUrl))
            {
                return base.HandleChallengeAsync(properties);
            }
            if (Options.LoginUrl.Contains("{{ReturnUrl}}", StringComparison.OrdinalIgnoreCase))
            {
                Response.Headers["Location"] = CreateLoginReturnUrl();
            }
            else
            {
                Response.Headers["Location"] = Options.LoginUrl;
            }
            Response.StatusCode = 302;
            return Task.CompletedTask;
        }

        private string CreateLoginReturnUrl()
        {
            var relativeUri = Request.GetEncodedPathAndQuery();
            var escaped = Uri.EscapeDataString(relativeUri);
            return Options.LoginUrl.Replace("{{ReturnUrl}}", escaped, StringComparison.OrdinalIgnoreCase);
        }
    }
}