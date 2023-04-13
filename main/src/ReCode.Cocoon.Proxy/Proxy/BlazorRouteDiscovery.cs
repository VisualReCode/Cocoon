using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReCode.Cocoon.Proxy.Proxy
{
    public static class BlazorRouteDiscovery
    {
        public static IEnumerable<string> FindRoutes(Type type)
        {
            var assembly = type.Assembly;
            foreach (var exportedType in assembly.GetExportedTypes())
            {
                if (exportedType.BaseType?.Name == "ComponentBase")
                {
                    // fix #31 - support multiple routeAttribute values
                    var routeAttributes = exportedType.GetCustomAttributes(typeof(RouteAttribute))
                        .Select(a => a as RouteAttribute);
                    if (!routeAttributes.Any()) continue;
                    foreach (var routeAttribute in routeAttributes)
                    {
                        if (routeAttribute != null)
                            yield return routeAttribute.Template;
                    }
                }
            }
        }
    }
}