using BenchmarkDotNet.Running;
using Benchmarks;

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
