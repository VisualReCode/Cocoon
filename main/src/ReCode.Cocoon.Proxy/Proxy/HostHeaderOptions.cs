using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCode.Cocoon.Proxy.Proxy
{
    /// <summary>
    /// Host Header options when proxying request
    /// </summary>
    /// <remarks>
    /// Fixes #30 https://github.com/VisualReCode/Cocoon/issues/30
    /// </remarks>
    public enum HostHeaderOptions
    {
        /// <summary>
        /// Default operation: no changes to Host header
        /// </summary>
        None,
        /// <summary>
        /// Used on Azure hosting: sets the Host header to `default` (null)
        /// </summary>
        SetDefault,
        /// <summary>
        /// Copies the host header from the request to the Cocoon proxy
        /// </summary>
        UseHost
    }
}
