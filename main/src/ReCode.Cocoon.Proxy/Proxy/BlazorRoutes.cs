using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReCode.Cocoon.Proxy.Proxy
{
    internal class BlazorRoutes
    {
        private readonly HashSet<string> _parameterless;
        private readonly Dictionary<string, RoutePart> _routes = new(StringComparer.OrdinalIgnoreCase);
        
        private readonly List<Regex> _parameters;
        public BlazorRoutes(IEnumerable<string> templates)
        {
            var templateArray = templates.ToArray();
            _parameterless = templateArray
                .Where(t => !t.Contains('{'))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            
            var parameterRegex = new Regex(@"\{.*?\}", RegexOptions.Compiled);

            foreach (var template in templateArray.Where(t => t.Contains('{')))
            {
                var parts = template.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (_routes.TryGetValue(parts[0], out var route))
                {
                    route.Add(parts.AsSpan().Slice(1));
                }
                else
                {
                    _routes.Add(parts[0], new RoutePart(parts.AsSpan().Slice(1)));
                }
                // _parameters ??= new List<Regex>();
                //
                // string expression;
                // if (template.Contains("{*"))
                // {
                //     expression = Regex.Replace(template, @"\{\*.*", ".*");
                // }
                // else
                // {
                //     expression = parameterRegex.Replace(template, @"[^\/]+");
                // }
                // _parameters.Add(new Regex($"^{expression}$", RegexOptions.Compiled | RegexOptions.IgnoreCase));
            }
        }

        public bool Contains(string path)
        {
            if (_parameterless.Contains(path)) return true;

            if (_routes.Count > 0)
            {
                var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (_routes.TryGetValue(parts[0], out var route))
                {
                    return route.IsMatch(parts.AsSpan().Slice(1));
                }
            }

            return false;
            //
            // if (_parameters is not null)
            // {
            //     foreach (var regex in _parameters)
            //     {
            //         if (regex.IsMatch(path))
            //         {
            //             return true;
            //         }
            //     }
            // }

            return false;
        }
    }
}