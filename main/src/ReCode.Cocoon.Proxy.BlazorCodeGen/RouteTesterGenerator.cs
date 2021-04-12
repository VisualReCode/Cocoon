using System;
using System.Collections.Generic;
using System.Linq;

namespace ReCode.Cocoon.Proxy.BlazorCodeGen
{
    public class RouteTesterGenerator
    {
        private readonly Route _root;

        public RouteTesterGenerator(IEnumerable<string> routes)
        {
            _root = new Route(true);

            foreach (var route in routes)
            {
                var parts = route.ToLowerInvariant()
                    .Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

                _root.Add(parts);
            }
        }

        public string Generate()
        {
            var code = new IndentedStringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine();
            code.AppendLine("namespace ReCode.Cocoon.Proxy.Blazor");
            using (code.OpenBrace())
            {
                code.AppendLine("public class CocoonBlazorRouteTester");
                using (code.OpenBrace())
                {
                    WriteIsMatch(code);
                    code.AppendLine();
                    WriteToNextSlash(code);
                }
            }

            return code.ToString();
        }

        private void WriteIsMatch(IndentedStringBuilder code)
        {
            code.AppendLine("public bool IsMatch(string path)");
            using (code.OpenBrace())
            {
                code.AppendLine("if (path == string.Empty) return false;");

                code.AppendLine("Span<char> span = stackalloc char[path.Length];");
                code.AppendLine("MemoryExtensions.ToLowerInvariant(path, span);");
                code.AppendLine("if (span[0] == '/' && span.Length > 1)");
                using (code.OpenBrace())
                {
                    code.AppendLine("span = span.Slice(1);");
                }

                code.AppendLine("var rest = ToNextSlash(ref span);");
                code.AppendLine("if (rest.Length == 0) return false;");
                code.AppendLine("var ch = rest[0];");
                code.AppendLine("rest = rest.Slice(1);");
                WriteSwitch(_root, code);
                code.AppendLine("return false;");
            }
        }

        private void WriteSwitch(Route route, IndentedStringBuilder code)
        {
            var values = route.GetValues()
                .Distinct()
                .ToArray();

            bool wildcard = values.Contains('*');
            values = values.Where(v => v != '*').ToArray();

            if (values.Length > 1)
            {
                code.AppendLine($"switch (ch)");
                using (code.OpenBrace())
                {
                    foreach (var c in values)
                    {
                        if (c == '*')
                        {
                            continue;
                        }

                        var subRoute = route.GetRoute(c);
                        code.AppendLine($"case '{c}':");
                        using (code.OpenBrace())
                        {
                            code.AppendLine("if (rest.Length == 0)");
                            using (code.OpenBrace())
                            {
                                if (subRoute.IsEnd)
                                {
                                    code.AppendLine("if (span.Length == 0) return true;");
                                }

                                if (subRoute.IsStart)
                                {
                                    if (subRoute.IsWildcard)
                                    {
                                        code.AppendLine("return true;");
                                    }
                                    else
                                    {
                                        code.AppendLine("rest = ToNextSlash(ref span);");
                                        code.AppendLine("ch = rest[0];");
                                        code.AppendLine("rest = rest.Slice(1);");
                                        WriteSwitch(subRoute, code);
                                    }
                                }

                                code.AppendLine("return false;");
                            }
                            code.AppendLine("else");
                            using (code.OpenBrace())
                            {
                                code.AppendLine("ch = rest[0];");
                                code.AppendLine("rest = rest.Slice(1);");
                                WriteSwitch(subRoute, code);
                            }
                            code.AppendLine("return false;");
                        }
                    }
                }
            }
            else if (values.Length == 1)
            {
                var c = values[0];
                var subRoute = route.GetRoute(c);
                code.AppendLine($"if (ch == '{c}')");
                using (code.OpenBrace())
                {
                    code.AppendLine("if (rest.Length == 0)");
                    using (code.OpenBrace())
                    {
                        if (subRoute.IsEnd)
                        {
                            code.AppendLine("if (span.Length == 0) return true;");
                        }

                        if (subRoute.IsStart)
                        {
                            if (subRoute.IsWildcard)
                            {
                                code.AppendLine("return true;");
                            }
                            else
                            {
                                code.AppendLine("rest = ToNextSlash(ref span);");
                                code.AppendLine("ch = rest[0];");
                                code.AppendLine("rest = rest.Slice(1);");
                                WriteSwitch(subRoute, code);
                            }
                        }

                        code.AppendLine("return false;");
                    }

                    code.AppendLine("ch = rest[0];");
                    code.AppendLine("rest = rest.Slice(1);");
                    WriteSwitch(subRoute, code);
                    code.AppendLine("return false;");
                }
            }

            if (wildcard)
            {
                var subRoute = route.GetRoute('*');
                code.AppendLine("rest = ToNextSlash(ref span);");
                if (subRoute.IsEnd)
                {
                    code.AppendLine("if (span.Length == 0) return true;");
                }
                code.AppendLine("ch = rest[0];");
            }
        }

        private static void WriteToNextSlash(IndentedStringBuilder code)
        {
            foreach (var line in ToNextSlash.Trim().Split('\n').Select(l => l.TrimEnd()))
            {
                code.AppendLine(line);
            }
        }

        private const string ToNextSlash = @"
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
";
    }
}