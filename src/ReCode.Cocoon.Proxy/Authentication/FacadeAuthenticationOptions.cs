using Microsoft.AspNetCore.Authentication;

namespace ReCode.Cocoon.Proxy.Authentication
{
    public class FacadeAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string BackendUrl { get; set; }
    }
}