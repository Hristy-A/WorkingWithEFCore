using BenchmarkDotNet.Attributes;
using Store.Infrastructure.HashProviders;

namespace Benchmarks
{
    public class HashProvidersBenchmarks
    {
        private BCryptHashProvider Bprovider = new BCryptHashProvider();
        private VanillaHashProvider Vprovider = new VanillaHashProvider();

        [Benchmark]
        public void BCryptGenerateHash() => Bprovider.GenerateHash("qwerty123qwerty456");

        [Benchmark]
        public void VanillaGenerateHash() => Vprovider.GenerateHash("qwerty123qwerty456");
    }
}
