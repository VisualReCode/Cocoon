using System;
using BenchmarkDotNet.Running;

namespace ReCode.Cocoon.Proxy.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BlazorRoutesBenchmarks>();
        }
    }
}