using System.Linq;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using ReCode.Cocoon.Proxy.Proxy;

namespace ReCode.Cocoon.Proxy.Benchmarks
{
    public class BlazorRoutesBenchmarks
    {
        private static readonly Regex[] Regexes =
        {
            new Regex(@"^\/$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Admin\/Edit\/.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Products\/.*$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Cats\/[^\/]+\/Breed", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Cats\/[^\/]+", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Dogs\/[^\/]+\/Breed", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Dogs\/[^\/]+", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Giraffes\/[^\/]+\/Breed", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Giraffes\/[^\/]+", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Penguins\/[^\/]+\/Breed", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Penguins\/[^\/]+", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/HoneyBadgers\/[^\/]+\/Breed", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/HoneyBadgers\/[^\/]+", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Fish\/[^\/]+\/Breed", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            new Regex(@"^\/Fish\/[^\/]+", RegexOptions.Compiled | RegexOptions.IgnoreCase),
        };
        
        private static readonly BlazorRoutes Routes = new(new[]
        {
            "/",
            "/Admin/Edit/{id}",
            "/Products/{*path}",
            "/Cats/{id}/Breed",
            "/Cats/{id}",
            "/Dogs/{id}/Breed",
            "/Dogs/{id}",
            "/Giraffes/{id}/Breed",
            "/Giraffes/{id}",
            "/Penguins/{id}/Breed",
            "/Penguins/{id}",
            "/HoneyBadgers/{id}/Breed",
            "/HoneyBadgers/{id}",
            "/Fish/{id}/Breed",
            "/Fish/{id}",
        });

        [Benchmark(Baseline = true)]
        public bool BlazorRoutes() => Routes.Contains("/fish/42");

        [Benchmark]
        public bool Regex() => Regexes.Any(r => r.IsMatch("/fish/42"));
    }
}