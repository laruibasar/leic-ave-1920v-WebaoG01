using System;

namespace WebaoBench
{
    class Program
    {



        public static Object call1()
        {
            //static readonly WebaoArtist artistWebao = (WebaoArtist)WebaoBuilder.Build(typeof(WebaoArtist), new HttpRequest());
            //Artist artist = artistWebao.GetInfo("muse");
            return null; 
        }

        static void Main(string[] args)
        {
            const long ITER_TIME = 1000;
            const long NUM_WARMUP = 10;
            const long NUM_ITER = 10;

            Console.WriteLine("START!");

            NBench.Benchmark(new BenchmarkMethod(call1), "call1", ITER_TIME, NUM_WARMUP, NUM_ITER);
                       
        }
    }
}
