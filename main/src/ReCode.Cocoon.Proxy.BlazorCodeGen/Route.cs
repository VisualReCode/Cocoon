using System;
using System.Collections.Generic;

namespace ReCode.Cocoon.Proxy.BlazorCodeGen
{
    internal class Route
    {
        private readonly Dictionary<char, Route> _routes = new();

        public Route(bool isStart)
        {
            IsStart = isStart;
        }

        public bool IsStart { get; }
        
        public bool IsEnd { get; private set; }
        public bool IsWildcard { get; private set; }

        public IEnumerable<char> GetValues() => _routes.Keys;
        public Route GetRoute(char key) => _routes[key];

        public void Add(Span<string> parts)
        {
            if (parts.Length == 0)
            {
                IsEnd = true;
                return;
            }
            var part = parts[0];
            
            if (parts[0].Length == 0)
            {
                if (parts.Length == 0)
                {
                    IsEnd = true;
                }
                return;
            }

            if (part[0] == '{')
            {
                if (part.Length > 1 && part[1] == '*')
                {
                    IsWildcard = true;
                    return;
                }

                char ch = '*';
                if (!_routes.TryGetValue(ch, out var route))
                {
                    route = new Route(part.Length == 1);
                    _routes.Add(ch, route);
                }
                route.Add(parts.Slice(1));
            }
            else
            {
                char ch = part[0];
                if (!_routes.TryGetValue(ch, out var route))
                {
                    route = new Route(part.Length == 1);
                    _routes.Add(ch, route);
                }

                if (part.Length == 1)
                {
                    route.Add(parts.Slice(1));
                }
                else
                {
                    parts[0] = part.Substring(1);
                    route.Add(parts);
                }
            }
            
        }
    }
}