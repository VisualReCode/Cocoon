using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ReCode.Cocoon.Proxy.Authentication
{
    [PublicAPI]
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
            var loginUrl = OptionsMonitor.CurrentValue.LoginUrl;
            
            if (string.IsNullOrEmpty(loginUrl))
            {
                return base.HandleChallengeAsync(properties);
            }
            if (loginUrl.Contains("{{ReturnUrl}}", StringComparison.OrdinalIgnoreCase))
            {
                Response.Headers["Location"] = CreateLoginReturnUrl(loginUrl);
            }
            else
            {
                Response.Headers["Location"] = loginUrl;
            }
            Response.StatusCode = 302;
            return Task.CompletedTask;
        }

        private string CreateLoginReturnUrl(string template)
        {
            var relativeUri = Request.GetEncodedPathAndQuery();
            var escaped = Uri.EscapeDataString(relativeUri);
            return template.Replace("{{ReturnUrl}}", escaped, StringComparison.OrdinalIgnoreCase);
        }
    }
}