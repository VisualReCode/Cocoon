using System;
using ReCode.Cocoon.Proxy.Blazor;
using Xunit;

namespace ReCode.Cocoon.Proxy.BlazorCodeGen.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void MatchesFoo()
        {
            var target = new CocoonBlazorRouteTester();
            Assert.True(target.IsMatch("/foo"));
        }

        [Fact]
        public void MatchesAdminPage()
        {
            var target = new CocoonBlazorRouteTester();
            Assert.True(target.IsMatch("/Admin/AdminPage"));
        }
    }
}
