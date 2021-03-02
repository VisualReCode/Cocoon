using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Components;

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
                    var routeAttribute = exportedType
                        .GetCustomAttribute(typeof(RouteAttribute))
                        as RouteAttribute;
                    if (routeAttribute is null) continue;
                    yield return routeAttribute.Template;
                }
            }
        }
    }
}