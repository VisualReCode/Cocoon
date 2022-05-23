using System.Net.Security;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public class CocoonProxyOptions
    {
        public RemoteCertificateValidationCallback? RemoteCertificateValidationCallback { get; set; }
    }
}
