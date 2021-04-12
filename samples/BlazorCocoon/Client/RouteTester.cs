using System;
using System.Collections.Generic;

namespace ReCode.Cocoon.Proxy.Blazor
{
    public class CocoonBlazorRouteTesterManual
    {
        public bool IsMatch(string path)
        {
            if (path == string.Empty) return false;
            Span<char> span = stackalloc char[path.Length];
            MemoryExtensions.ToLowerInvariant(path, span);
            if (span[0] == '/' && span.Length > 1)
            {
                span = span.Slice(1);
            }
            var rest = ToNextSlash(ref span);
            if (rest.Length == 0) return false;
            var ch = rest[0];
            rest = rest.Slice(1);
            switch (ch)
            {
                case 'a':
                {
                    if (rest.Length == 0)
                    {
                        return false;
                    }
                    else
                    {
                        ch = rest[0];
                        rest = rest.Slice(1);
                        if (ch == 'd')
                        {
                            if (rest.Length == 0)
                            {
                                return false;
                            }
                            ch = rest[0];
                            rest = rest.Slice(1);
                            if (ch == 'm')
                            {
                                if (rest.Length == 0)
                                {
                                    return false;
                                }
                                ch = rest[0];
                                rest = rest.Slice(1);
                                if (ch == 'i')
                                {
                                    if (rest.Length == 0)
                                    {
                                        return false;
                                    }
                                    ch = rest[0];
                                    rest = rest.Slice(1);
                                    if (ch == 'n')
                                    {
                                        if (rest.Length == 0)
                                        {
                                            rest = ToNextSlash(ref span);
                                            ch = rest[0];
                                            switch (ch)
                                            {
                                                case 'a':
                                                {
                                                    if (rest.Length == 0)
                                                    {
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        ch = rest[0];
                                                        rest = rest.Slice(1);
                                                        if (ch == 'd')
                                                        {
                                                            if (rest.Length == 0)
                                                            {
                                                                return false;
                                                            }
                                                            ch = rest[0];
                                                            rest = rest.Slice(1);
                                                            if (ch == 'm')
                                                            {
                                                                if (rest.Length == 0)
                                                                {
                                                                    return false;
                                                                }
                                                                ch = rest[0];
                                                                rest = rest.Slice(1);
                                                                if (ch == 'i')
                                                                {
                                                                    if (rest.Length == 0)
                                                                    {
                                                                        return false;
                                                                    }
                                                                    ch = rest[0];
                                                                    rest = rest.Slice(1);
                                                                    if (ch == 'n')
                                                                    {
                                                                        if (rest.Length == 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        ch = rest[0];
                                                                        rest = rest.Slice(1);
                                                                        if (ch == 'p')
                                                                        {
                                                                            if (rest.Length == 0)
                                                                            {
                                                                                return false;
                                                                            }
                                                                            ch = rest[0];
                                                                            rest = rest.Slice(1);
                                                                            if (ch == 'a')
                                                                            {
                                                                                if (rest.Length == 0)
                                                                                {
                                                                                    return false;
                                                                                }
                                                                                ch = rest[0];
                                                                                rest = rest.Slice(1);
                                                                                if (ch == 'g')
                                                                                {
                                                                                    if (rest.Length == 0)
                                                                                    {
                                                                                        return false;
                                                                                    }
                                                                                    ch = rest[0];
                                                                                    rest = rest.Slice(1);
                                                                                    if (ch == 'e')
                                                                                    {
                                                                                        if (rest.Length == 0)
                                                                                        {
                                                                                            if (span.Length == 0) return true;
                                                                                            rest = ToNextSlash(ref span);
                                                                                            ch = rest[0];
                                                                                            return false;
                                                                                        }
                                                                                        ch = rest[0];
                                                                                        rest = rest.Slice(1);
                                                                                        return false;
                                                                                    }
                                                                                    return false;
                                                                                }
                                                                                return false;
                                                                            }
                                                                            return false;
                                                                        }
                                                                        return false;
                                                                    }
                                                                    return false;
                                                                }
                                                                return false;
                                                            }
                                                            return false;
                                                        }
                                                    }
                                                    return false;
                                                }
                                                case 'e':
                                                {
                                                    if (rest.Length == 0)
                                                    {
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        ch = rest[0];
                                                        rest = rest.Slice(1);
                                                        if (ch == 'd')
                                                        {
                                                            if (rest.Length == 0)
                                                            {
                                                                return false;
                                                            }
                                                            ch = rest[0];
                                                            rest = rest.Slice(1);
                                                            if (ch == 'i')
                                                            {
                                                                if (rest.Length == 0)
                                                                {
                                                                    return false;
                                                                }
                                                                ch = rest[0];
                                                                rest = rest.Slice(1);
                                                                if (ch == 't')
                                                                {
                                                                    if (rest.Length == 0)
                                                                    {
                                                                        return false;
                                                                    }
                                                                    ch = rest[0];
                                                                    rest = rest.Slice(1);
                                                                    if (ch == 'p')
                                                                    {
                                                                        if (rest.Length == 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        ch = rest[0];
                                                                        rest = rest.Slice(1);
                                                                        if (ch == 'r')
                                                                        {
                                                                            if (rest.Length == 0)
                                                                            {
                                                                                return false;
                                                                            }
                                                                            ch = rest[0];
                                                                            rest = rest.Slice(1);
                                                                            if (ch == 'o')
                                                                            {
                                                                                if (rest.Length == 0)
                                                                                {
                                                                                    return false;
                                                                                }
                                                                                ch = rest[0];
                                                                                rest = rest.Slice(1);
                                                                                if (ch == 'd')
                                                                                {
                                                                                    if (rest.Length == 0)
                                                                                    {
                                                                                        return false;
                                                                                    }
                                                                                    ch = rest[0];
                                                                                    rest = rest.Slice(1);
                                                                                    if (ch == 'u')
                                                                                    {
                                                                                        if (rest.Length == 0)
                                                                                        {
                                                                                            return false;
                                                                                        }
                                                                                        ch = rest[0];
                                                                                        rest = rest.Slice(1);
                                                                                        if (ch == 'c')
                                                                                        {
                                                                                            if (rest.Length == 0)
                                                                                            {
                                                                                                return false;
                                                                                            }
                                                                                            ch = rest[0];
                                                                                            rest = rest.Slice(1);
                                                                                            if (ch == 't')
                                                                                            {
                                                                                                if (rest.Length == 0)
                                                                                                {
                                                                                                    rest = ToNextSlash(ref span);
                                                                                                    ch = rest[0];
                                                                                                    rest = ToNextSlash(ref span);
                                                                                                    if (span.Length == 0) return true;
                                                                                                    ch = rest[0];
                                                                                                    return false;
                                                                                                }
                                                                                                ch = rest[0];
                                                                                                rest = rest.Slice(1);
                                                                                                rest = ToNextSlash(ref span);
                                                                                                if (span.Length == 0) return true;
                                                                                                ch = rest[0];
                                                                                                return false;
                                                                                            }
                                                                                            return false;
                                                                                        }
                                                                                        return false;
                                                                                    }
                                                                                    return false;
                                                                                }
                                                                                return false;
                                                                            }
                                                                            return false;
                                                                        }
                                                                        return false;
                                                                    }
                                                                    return false;
                                                                }
                                                                return false;
                                                            }
                                                            return false;
                                                        }
                                                    }
                                                    return false;
                                                }
                                                case 'n':
                                                {
                                                    if (rest.Length == 0)
                                                    {
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        ch = rest[0];
                                                        rest = rest.Slice(1);
                                                        if (ch == 'e')
                                                        {
                                                            if (rest.Length == 0)
                                                            {
                                                                return false;
                                                            }
                                                            ch = rest[0];
                                                            rest = rest.Slice(1);
                                                            if (ch == 'w')
                                                            {
                                                                if (rest.Length == 0)
                                                                {
                                                                    return false;
                                                                }
                                                                ch = rest[0];
                                                                rest = rest.Slice(1);
                                                                if (ch == 'p')
                                                                {
                                                                    if (rest.Length == 0)
                                                                    {
                                                                        return false;
                                                                    }
                                                                    ch = rest[0];
                                                                    rest = rest.Slice(1);
                                                                    if (ch == 'r')
                                                                    {
                                                                        if (rest.Length == 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        ch = rest[0];
                                                                        rest = rest.Slice(1);
                                                                        if (ch == 'o')
                                                                        {
                                                                            if (rest.Length == 0)
                                                                            {
                                                                                return false;
                                                                            }
                                                                            ch = rest[0];
                                                                            rest = rest.Slice(1);
                                                                            if (ch == 'd')
                                                                            {
                                                                                if (rest.Length == 0)
                                                                                {
                                                                                    return false;
                                                                                }
                                                                                ch = rest[0];
                                                                                rest = rest.Slice(1);
                                                                                if (ch == 'u')
                                                                                {
                                                                                    if (rest.Length == 0)
                                                                                    {
                                                                                        return false;
                                                                                    }
                                                                                    ch = rest[0];
                                                                                    rest = rest.Slice(1);
                                                                                    if (ch == 'c')
                                                                                    {
                                                                                        if (rest.Length == 0)
                                                                                        {
                                                                                            return false;
                                                                                        }
                                                                                        ch = rest[0];
                                                                                        rest = rest.Slice(1);
                                                                                        if (ch == 't')
                                                                                        {
                                                                                            if (rest.Length == 0)
                                                                                            {
                                                                                                if (span.Length == 0) return true;
                                                                                                rest = ToNextSlash(ref span);
                                                                                                ch = rest[0];
                                                                                                return false;
                                                                                            }
                                                                                            ch = rest[0];
                                                                                            rest = rest.Slice(1);
                                                                                            return false;
                                                                                        }
                                                                                        return false;
                                                                                    }
                                                                                    return false;
                                                                                }
                                                                                return false;
                                                                            }
                                                                            return false;
                                                                        }
                                                                        return false;
                                                                    }
                                                                    return false;
                                                                }
                                                                return false;
                                                            }
                                                            return false;
                                                        }
                                                    }
                                                    return false;
                                                }
                                            }
                                            return false;
                                        }
                                        ch = rest[0];
                                        rest = rest.Slice(1);
                                        switch (ch)
                                        {
                                            case 'a':
                                            {
                                                if (rest.Length == 0)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    ch = rest[0];
                                                    rest = rest.Slice(1);
                                                    if (ch == 'd')
                                                    {
                                                        if (rest.Length == 0)
                                                        {
                                                            return false;
                                                        }
                                                        ch = rest[0];
                                                        rest = rest.Slice(1);
                                                        if (ch == 'm')
                                                        {
                                                            if (rest.Length == 0)
                                                            {
                                                                return false;
                                                            }
                                                            ch = rest[0];
                                                            rest = rest.Slice(1);
                                                            if (ch == 'i')
                                                            {
                                                                if (rest.Length == 0)
                                                                {
                                                                    return false;
                                                                }
                                                                ch = rest[0];
                                                                rest = rest.Slice(1);
                                                                if (ch == 'n')
                                                                {
                                                                    if (rest.Length == 0)
                                                                    {
                                                                        return false;
                                                                    }
                                                                    ch = rest[0];
                                                                    rest = rest.Slice(1);
                                                                    if (ch == 'p')
                                                                    {
                                                                        if (rest.Length == 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        ch = rest[0];
                                                                        rest = rest.Slice(1);
                                                                        if (ch == 'a')
                                                                        {
                                                                            if (rest.Length == 0)
                                                                            {
                                                                                return false;
                                                                            }
                                                                            ch = rest[0];
                                                                            rest = rest.Slice(1);
                                                                            if (ch == 'g')
                                                                            {
                                                                                if (rest.Length == 0)
                                                                                {
                                                                                    return false;
                                                                                }
                                                                                ch = rest[0];
                                                                                rest = rest.Slice(1);
                                                                                if (ch == 'e')
                                                                                {
                                                                                    if (rest.Length == 0)
                                                                                    {
                                                                                        if (span.Length == 0) return true;
                                                                                        rest = ToNextSlash(ref span);
                                                                                        ch = rest[0];
                                                                                        return false;
                                                                                    }
                                                                                    ch = rest[0];
                                                                                    rest = rest.Slice(1);
                                                                                    return false;
                                                                                }
                                                                                return false;
                                                                            }
                                                                            return false;
                                                                        }
                                                                        return false;
                                                                    }
                                                                    return false;
                                                                }
                                                                return false;
                                                            }
                                                            return false;
                                                        }
                                                        return false;
                                                    }
                                                }
                                                return false;
                                            }
                                            case 'e':
                                            {
                                                if (rest.Length == 0)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    ch = rest[0];
                                                    rest = rest.Slice(1);
                                                    if (ch == 'd')
                                                    {
                                                        if (rest.Length == 0)
                                                        {
                                                            return false;
                                                        }
                                                        ch = rest[0];
                                                        rest = rest.Slice(1);
                                                        if (ch == 'i')
                                                        {
                                                            if (rest.Length == 0)
                                                            {
                                                                return false;
                                                            }
                                                            ch = rest[0];
                                                            rest = rest.Slice(1);
                                                            if (ch == 't')
                                                            {
                                                                if (rest.Length == 0)
                                                                {
                                                                    return false;
                                                                }
                                                                ch = rest[0];
                                                                rest = rest.Slice(1);
                                                                if (ch == 'p')
                                                                {
                                                                    if (rest.Length == 0)
                                                                    {
                                                                        return false;
                                                                    }
                                                                    ch = rest[0];
                                                                    rest = rest.Slice(1);
                                                                    if (ch == 'r')
                                                                    {
                                                                        if (rest.Length == 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        ch = rest[0];
                                                                        rest = rest.Slice(1);
                                                                        if (ch == 'o')
                                                                        {
                                                                            if (rest.Length == 0)
                                                                            {
                                                                                return false;
                                                                            }
                                                                            ch = rest[0];
                                                                            rest = rest.Slice(1);
                                                                            if (ch == 'd')
                                                                            {
                                                                                if (rest.Length == 0)
                                                                                {
                                                                                    return false;
                                                                                }
                                                                                ch = rest[0];
                                                                                rest = rest.Slice(1);
                                                                                if (ch == 'u')
                                                                                {
                                                                                    if (rest.Length == 0)
                                                                                    {
                                                                                        return false;
                                                                                    }
                                                                                    ch = rest[0];
                                                                                    rest = rest.Slice(1);
                                                                                    if (ch == 'c')
                                                                                    {
                                                                                        if (rest.Length == 0)
                                                                                        {
                                                                                            return false;
                                                                                        }
                                                                                        ch = rest[0];
                                                                                        rest = rest.Slice(1);
                                                                                        if (ch == 't')
                                                                                        {
                                                                                            if (rest.Length == 0)
                                                                                            {
                                                                                                rest = ToNextSlash(ref span);
                                                                                                ch = rest[0];
                                                                                                rest = ToNextSlash(ref span);
                                                                                                if (span.Length == 0) return true;
                                                                                                ch = rest[0];
                                                                                                return false;
                                                                                            }
                                                                                            ch = rest[0];
                                                                                            rest = rest.Slice(1);
                                                                                            rest = ToNextSlash(ref span);
                                                                                            if (span.Length == 0) return true;
                                                                                            ch = rest[0];
                                                                                            return false;
                                                                                        }
                                                                                        return false;
                                                                                    }
                                                                                    return false;
                                                                                }
                                                                                return false;
                                                                            }
                                                                            return false;
                                                                        }
                                                                        return false;
                                                                    }
                                                                    return false;
                                                                }
                                                                return false;
                                                            }
                                                            return false;
                                                        }
                                                        return false;
                                                    }
                                                }
                                                return false;
                                            }
                                            case 'n':
                                            {
                                                if (rest.Length == 0)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    ch = rest[0];
                                                    rest = rest.Slice(1);
                                                    if (ch == 'e')
                                                    {
                                                        if (rest.Length == 0)
                                                        {
                                                            return false;
                                                        }
                                                        ch = rest[0];
                                                        rest = rest.Slice(1);
                                                        if (ch == 'w')
                                                        {
                                                            if (rest.Length == 0)
                                                            {
                                                                return false;
                                                            }
                                                            ch = rest[0];
                                                            rest = rest.Slice(1);
                                                            if (ch == 'p')
                                                            {
                                                                if (rest.Length == 0)
                                                                {
                                                                    return false;
                                                                }
                                                                ch = rest[0];
                                                                rest = rest.Slice(1);
                                                                if (ch == 'r')
                                                                {
                                                                    if (rest.Length == 0)
                                                                    {
                                                                        return false;
                                                                    }
                                                                    ch = rest[0];
                                                                    rest = rest.Slice(1);
                                                                    if (ch == 'o')
                                                                    {
                                                                        if (rest.Length == 0)
                                                                        {
                                                                            return false;
                                                                        }
                                                                        ch = rest[0];
                                                                        rest = rest.Slice(1);
                                                                        if (ch == 'd')
                                                                        {
                                                                            if (rest.Length == 0)
                                                                            {
                                                                                return false;
                                                                            }
                                                                            ch = rest[0];
                                                                            rest = rest.Slice(1);
                                                                            if (ch == 'u')
                                                                            {
                                                                                if (rest.Length == 0)
                                                                                {
                                                                                    return false;
                                                                                }
                                                                                ch = rest[0];
                                                                                rest = rest.Slice(1);
                                                                                if (ch == 'c')
                                                                                {
                                                                                    if (rest.Length == 0)
                                                                                    {
                                                                                        return false;
                                                                                    }
                                                                                    ch = rest[0];
                                                                                    rest = rest.Slice(1);
                                                                                    if (ch == 't')
                                                                                    {
                                                                                        if (rest.Length == 0)
                                                                                        {
                                                                                            if (span.Length == 0) return true;
                                                                                            rest = ToNextSlash(ref span);
                                                                                            ch = rest[0];
                                                                                            return false;
                                                                                        }
                                                                                        ch = rest[0];
                                                                                        rest = rest.Slice(1);
                                                                                        return false;
                                                                                    }
                                                                                    return false;
                                                                                }
                                                                                return false;
                                                                            }
                                                                            return false;
                                                                        }
                                                                        return false;
                                                                    }
                                                                    return false;
                                                                }
                                                                return false;
                                                            }
                                                            return false;
                                                        }
                                                        return false;
                                                    }
                                                }
                                                return false;
                                            }
                                        }
                                        return false;
                                    }
                                    return false;
                                }
                                return false;
                            }
                            return false;
                        }
                    }
                    return false;
                }
                case 'c':
                {
                    if (rest.Length == 0)
                    {
                        return false;
                    }
                    else
                    {
                        ch = rest[0];
                        rest = rest.Slice(1);
                        if (ch == 'o')
                        {
                            if (rest.Length == 0)
                            {
                                return false;
                            }
                            ch = rest[0];
                            rest = rest.Slice(1);
                            if (ch == 'u')
                            {
                                if (rest.Length == 0)
                                {
                                    return false;
                                }
                                ch = rest[0];
                                rest = rest.Slice(1);
                                if (ch == 'n')
                                {
                                    if (rest.Length == 0)
                                    {
                                        return false;
                                    }
                                    ch = rest[0];
                                    rest = rest.Slice(1);
                                    if (ch == 't')
                                    {
                                        if (rest.Length == 0)
                                        {
                                            return false;
                                        }
                                        ch = rest[0];
                                        rest = rest.Slice(1);
                                        if (ch == 'e')
                                        {
                                            if (rest.Length == 0)
                                            {
                                                return false;
                                            }
                                            ch = rest[0];
                                            rest = rest.Slice(1);
                                            if (ch == 'r')
                                            {
                                                if (rest.Length == 0)
                                                {
                                                    if (span.Length == 0) return true;
                                                    rest = ToNextSlash(ref span);
                                                    ch = rest[0];
                                                    return false;
                                                }
                                                ch = rest[0];
                                                rest = rest.Slice(1);
                                                return false;
                                            }
                                            return false;
                                        }
                                        return false;
                                    }
                                    return false;
                                }
                                return false;
                            }
                            return false;
                        }
                    }
                    return false;
                }
                case 'f':
                {
                    if (rest.Length == 0)
                    {
                        return false;
                    }
                    else
                    {
                        ch = rest[0];
                        rest = rest.Slice(1);
                        if (ch == 'e')
                        {
                            if (rest.Length == 0)
                            {
                                return false;
                            }
                            ch = rest[0];
                            rest = rest.Slice(1);
                            if (ch == 't')
                            {
                                if (rest.Length == 0)
                                {
                                    return false;
                                }
                                ch = rest[0];
                                rest = rest.Slice(1);
                                if (ch == 'c')
                                {
                                    if (rest.Length == 0)
                                    {
                                        return false;
                                    }
                                    ch = rest[0];
                                    rest = rest.Slice(1);
                                    if (ch == 'h')
                                    {
                                        if (rest.Length == 0)
                                        {
                                            return false;
                                        }
                                        ch = rest[0];
                                        rest = rest.Slice(1);
                                        if (ch == 'd')
                                        {
                                            if (rest.Length == 0)
                                            {
                                                return false;
                                            }
                                            ch = rest[0];
                                            rest = rest.Slice(1);
                                            if (ch == 'a')
                                            {
                                                if (rest.Length == 0)
                                                {
                                                    return false;
                                                }
                                                ch = rest[0];
                                                rest = rest.Slice(1);
                                                if (ch == 't')
                                                {
                                                    if (rest.Length == 0)
                                                    {
                                                        return false;
                                                    }
                                                    ch = rest[0];
                                                    rest = rest.Slice(1);
                                                    if (ch == 'a')
                                                    {
                                                        if (rest.Length == 0)
                                                        {
                                                            if (span.Length == 0) return true;
                                                            rest = ToNextSlash(ref span);
                                                            ch = rest[0];
                                                            return false;
                                                        }
                                                        ch = rest[0];
                                                        rest = rest.Slice(1);
                                                        return false;
                                                    }
                                                    return false;
                                                }
                                                return false;
                                            }
                                            return false;
                                        }
                                        return false;
                                    }
                                    return false;
                                }
                                return false;
                            }
                            return false;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        private static Span<char> ToNextSlash(ref Span<char> span)
        {
            var index = span.IndexOf('/');
            if (index > 0)
            {
                var ret = span.Slice(0, index);
                span = span.Slice(index + 1);
                return ret;
            }
            else
            {
                var ret = span;
                span = Array.Empty<char>().AsSpan();
                return ret;
            }
        }
    }
}
