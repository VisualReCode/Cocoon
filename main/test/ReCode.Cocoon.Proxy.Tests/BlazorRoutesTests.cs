using System;
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
    }
}
