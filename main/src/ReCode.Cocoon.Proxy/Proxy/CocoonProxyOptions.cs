using System.Net.Security;
using JetBrains.Annotations;

namespace ReCode.Cocoon.Proxy.Proxy
{
    [UsedImplicitly]
    public class CocoonProxyOptions
    {
        public RemoteCertificateValidationCallback? RemoteCertificateValidationCallback { get; set; }
    }
}
