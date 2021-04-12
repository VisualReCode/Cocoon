using System;

namespace Microsoft.AspNetCore.Components
{
    public class RouteAttribute : Attribute
    {
        private readonly string _template;

        public RouteAttribute(string template)
        {
            _template = template;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RouteTesterAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public RouteTesterAttribute(params Type[] types)
        {
        }
    }
}