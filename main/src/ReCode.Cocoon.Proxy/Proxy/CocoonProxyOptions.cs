using System.Net.Security;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public class CocoonProxyOptions
    {
        public RemoteCertificateValidationCallback? RemoteCertificateValidationCallback { get; set; }

        /// <summary>
        /// Set Host header behaviour
        /// </summary>
        public HostHeaderOptions HostHeaderOption { get; set; } = HostHeaderOptions.None;
    }
}
