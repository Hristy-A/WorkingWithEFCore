using BenchmarkDotNet.Attributes;
using Store.Infrastructure.HashProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
