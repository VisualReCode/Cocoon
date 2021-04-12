using System;

namespace ReCode.Cocoon.Proxy.Blazor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RouteTesterAttribute : Attribute
    {
        public RouteTesterAttribute(params Type[] types)
        {
        }
    }
}