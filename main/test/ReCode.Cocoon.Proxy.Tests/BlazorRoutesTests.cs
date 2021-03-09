using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ReCode.Cocoon.Proxy.Proxy;
using Xunit;

namespace ReCode.Cocoon.Proxy.Tests
{
    public class BlazorRoutesTests
    {
        [Fact]
        public void MatchesTemplateWithParameters()
        {
            string[] templates =
            {
                "/",
                "/Admin/Edit/{id}",
                "/Products/{*path}",
                "/Cats/{id}/Breed",
                "/Cats/{id}",
            };
            var target = new BlazorRoutes(templates);
            Assert.True(target.Contains("/admin/edit/42"));
            Assert.True(target.Contains("/products/foo/bar/quux"));
            Assert.True(target.Contains("cats/42/breed"));
            Assert.True(target.Contains("cats/42"));
            Assert.True(target.Contains("/"));
            
            Assert.False(target.Contains("/admin/edit/42/wibble"));
            Assert.False(target.Contains("/fail"));
        }
        
        [Fact]
        public void MatchesTemplateWithRegexes()
        {
            Regex[] templates =
            {
                new Regex(@"^\/$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                new Regex(@"^\/Admin\/Edit\/[^\/]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                new Regex(@"^\/Products\/.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                new Regex(@"^\/Cats\/[^\/]+\/Breed$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                new Regex(@"^\/Cats\/[^\/]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            };
            Assert.True(templates.Any(r => r.IsMatch("/admin/edit/42")));
            Assert.True(templates.Any(r => r.IsMatch("/products/foo/bar/quux")));
            Assert.True(templates.Any(r => r.IsMatch("/cats/42/breed")));
            Assert.True(templates.Any(r => r.IsMatch("/cats/42")));
            Assert.True(templates.Any(r => r.IsMatch("/")));
            
            Assert.False(templates.Any(r => r.IsMatch("/admin/edit/42/wibble")));
            Assert.False(templates.Any(r => r.IsMatch("/fail")));
        }

        [Fact]
        public void MemoryEquality()
        {
            var hashSet = new HashSet<PathChunk>(PathChunkComparer.Instance);
            hashSet.Add(new PathChunk("foo".AsMemory()));
            var astr = "foo";
            Assert.True(hashSet.Contains(new PathChunk(astr.AsMemory())));
        }
    }

    public readonly struct PathChunk
    {
        private readonly ReadOnlyMemory<char> _text;
        private readonly int _hashCode;

        public PathChunk(ReadOnlyMemory<char> text)
        {
            _text = text;
            _hashCode = GetHashCode(text);
        }

        public ReadOnlySpan<char> Span => _text.Span;

        public override int GetHashCode() => _hashCode;

        private static int GetHashCode(ReadOnlyMemory<char> obj)
        {
            int code = 0;
            var span = obj.Span;
            while (span.Length > 0)
            {
                code = HashCode.Combine(code, span[0]);
                span = span.Slice(1);
            }

            return code;
        }
    }
        
    
    public class PathChunkComparer : IEqualityComparer<PathChunk>
    {
        public static readonly PathChunkComparer Instance = new();
        public bool Equals(PathChunk x, PathChunk y)
        {
            // return MemoryExtensions.Equals(x.Span, y.Span, StringComparison.OrdinalIgnoreCase);
            return x.Span.SequenceEqual(y.Span);
        }

        public int GetHashCode(PathChunk obj)
        {
            return obj.GetHashCode();
        }
    }
}
