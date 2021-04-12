using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ReCode.Cocoon.Proxy.BlazorCodeGen;
using Xunit;
using Xunit.Abstractions;

namespace RouteTesterGen.Tests
{
    public class RouteTesterGeneratorTests
    {
        private readonly ITestOutputHelper _output;
        private Func<string, bool> _isMatch;

        public RouteTesterGeneratorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("/posts")]
        [InlineData("/posts/42")]
        [InlineData("/posts/42/edit")]
        [InlineData("/people")]
        [InlineData("/people/42")]
        [InlineData("/people/42/edit")]
        [InlineData("/files/foo/bar/quux.pdf")]
        [InlineData("/Admin/AdminPage")]
        public void MatchesPath(string path)
        {
            _isMatch ??= Build();
            Assert.True(_isMatch(path));
        }

        private Func<string, bool> Build()
        {
            var routes = new[]
            {
                "/posts",
                "/posts/{id}",
                "/posts/{id}/edit",
                "/people",
                "/people/{id}",
                "/people/{id}/edit",
                "/files/{*path}",
                "/Admin/AdminPage",
            };
            var target = new RouteTesterGenerator(routes);
            var actual = target.Generate();

            var assemblyName = Path.GetRandomFileName();
            var references = new[]
                {
                    typeof(object).Assembly.Location
                }.Select(p => MetadataReference.CreateFromFile(p))
                .ToArray();
            var syntax = CSharpSyntaxTree.ParseText(actual);
            var compilation = CSharpCompilation.Create(assemblyName, new[] {syntax},
                references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (result.Diagnostics.Length > 0)
            {
                foreach (var diagnostic in result.Diagnostics)
                {
                    _output.WriteLine(diagnostic.ToString());
                }
            }
            Assert.True(result.Success);
            ms.Position = 0;
            var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            var type = assembly.GetType("ReCode.Cocoon.Proxy.Blazor.CocoonBlazorRouteTester");
            Assert.NotNull(type);
            var method = type.GetMethod("IsMatch");
            Assert.NotNull(method);
            var obj = Activator.CreateInstance(type);
            return method.CreateDelegate<Func<string, bool>>(obj);
        }
    }
}