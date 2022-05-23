using JetBrains.Annotations;

namespace ReCode.Cocoon.Proxy.Session
{
    [PublicAPI]
    public class CocoonSessionOptions
    {
        public string BackendApiUrl { get; set; } = null!;
        
        public string[]? Headers { get; set; }
        public string[] Cookies { get; set; } = null!;
    }
}