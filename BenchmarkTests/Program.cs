using BenchmarkDotNet.Running;
using Benchmarks;
using System;

namespace BenchmarkTests
{
    internal class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<HashProvidersBenchmarks>();
        }
    }
}
