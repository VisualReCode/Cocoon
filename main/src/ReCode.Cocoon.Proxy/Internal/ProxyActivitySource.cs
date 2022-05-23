using System.Diagnostics;

namespace ReCode.Cocoon.Proxy;

internal static class ProxyActivitySource
{
    private static readonly ActivitySource Source = new("ReCode.Cocoon.Proxy");

    internal static Activity? StartActivity(string name) => Source.StartActivity(name, ActivityKind.Client);
}