using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace libplctag.NativeImport.Benchmarks
{
    [MemoryDiagnoser]
    public class GetStatusRawVsExtractFirst
    {

        private const int N = 1000;

        private readonly int[] tagHandles;

        public GetStatusRawVsExtractFirst()
        {

            tagHandles = new int[N];
            for (int ii = 0; ii < N; ii++)
            {
                tagHandles[ii] = plctag.plc_tag_create("protocol=ab_eip&gateway=192.168.0.10&path=1,0&plc=LGX&elem_size=4&elem_count=1&name=MY_DINT", 1000);
            }
        }

        [Benchmark]
        public void plc_tag_status()
        {
            for (int ii = 0; ii < N; ii++)
            {
                plctag.plc_tag_status(tagHandles[ii]);
            }
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<GetStatusRawVsExtractFirst>();
        }
    }
}
