using System;
using System.Collections.Generic;
using System.Text;
using Webao;
using WebaoDynamic;
using WebaoDynDummy;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoBenchMark
{
    class ArtistBench
    {
		static readonly IWebaoArtist webaoArtistBuild = (IWebaoArtist)WebaoDynBuilder.Build(typeof(IWebaoArtist), new LastfmMockRequest());
		static readonly IWebaoArtist webaoArtistEmmit = (IWebaoArtist)WebaoEmitter.ConstructorEmitter(typeof(IWebaoArtist), new LastfmMockRequest());
		
		public static Object testBuildArtistGetInfo()
		{
			return webaoArtistBuild.GetInfo("muse");
		}
		public static Object testBuildArtistSearch()
		{
			return webaoArtistBuild.Search("black", 1);
		}

		public static Object testEmmitArtistGetInfo()
		{
			return webaoArtistEmmit.GetInfo("muse");
		}
		public static Object testEmmitdArtistSearch()
		{
			return webaoArtistEmmit.Search("black", 1);
		}

		public static void Main()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testBuildArtistGetInfo), "Build Artist GetInfo", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testEmmitArtistGetInfo), "Emmit Artist GetInfo", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testBuildArtistSearch), " Build Artist Search", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testBuildArtistSearch), "Emmit Artist Search", ITER_TIME, NUM_WARMUP, NUM_ITER);

		}
	}
}
