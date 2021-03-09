using System;
using System.Collections.Generic;

namespace ReCode.Cocoon.Proxy.Proxy
{
    internal class RoutePart
    {
        private readonly Dictionary<string, RoutePart> _subRoutes =
            new(StringComparer.OrdinalIgnoreCase);

        private RoutePart? _subRoute;

        private bool _isMatch;
        private bool _isEnd;

        public RoutePart(Span<string> parts)
        {
            if (parts.Length == 0)
            {
                _isEnd = true;
            }
            else if (parts[0].StartsWith("{*"))
            {
                _isMatch = true;
            }
            else if (parts[0].StartsWith('{'))
            {
                if (parts.Length == 1)
                {
                    _isEnd = true;
                }

                _subRoute = new RoutePart(parts.Slice(1));
            }
            else
            {
                _subRoutes.Add(parts[0], new RoutePart(parts.Slice(1)));
            }
        }

        public void Add(Span<string> parts)
        {
            if (_isMatch) return;
            if (parts[0].StartsWith('{'))
            {
                if (parts[0].StartsWith("{*"))
                {
                    _isMatch = true;
                }
                else if (parts.Length == 1)
                {
                    _isEnd = true;
                }
                else
                {
                    _subRoute = new RoutePart(parts.Slice(1));
                }
            }
            else
            {
                if (_subRoutes.TryGetValue(parts[0], out var subRoute))
                {
                    subRoute.Add(parts.Slice(1));
                }
                else
                {
                    _subRoutes.Add(parts[0], new RoutePart(parts.Slice(1)));
                }
            }
        }

        public bool IsMatch(Span<string> parts)
        {
            if (_isMatch) return true;

            if (_isEnd && parts.Length == 0)
            {
                return true;
            }
            
            if (_subRoute is not null)
            {
                if (parts.Length == 1)
                {
                    return true;
                }
                return _subRoute.IsMatch(parts.Slice(1));
            }
            if (_subRoutes.TryGetValue(parts[0], out var subRoute))
            {
                if (parts.Length == 1)
                {
                    return true;
                }
                return subRoute.IsMatch(parts.Slice(1));
            }

            return false;
        }
    }
}