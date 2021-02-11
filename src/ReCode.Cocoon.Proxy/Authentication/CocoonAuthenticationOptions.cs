using Microsoft.AspNetCore.Authentication;

namespace ReCode.Cocoon.Proxy.Authentication
{
    public class CocoonAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string BackendApiUrl { get; set; } = null!;
        
        public string? LoginUrl { get; set; }
        
        public string[]? Headers { get; set; }
        
        public string[]? Cookies { get; set; }
    }
}