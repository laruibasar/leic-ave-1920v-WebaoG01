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
		static IWebaoArtist webaoArtistDyn = (IWebaoArtist)WebaoDynamic.WebaoDynBuilder.Build(typeof(IWebaoArtist), new LastfmMockRequest());
		static WebaoArtist webaoArtistReflect = (WebaoArtist)WebaoBuilder.Build(typeof(WebaoArtist), new LastfmMockRequest());

		public static Object testReflectArtistGetInfo()
		{
			Artist artist = webaoArtistReflect.GetInfo("muse");
			return artist;
		}
		public static Object testReflectArtistSearch()
		{
			List<Artist> artists = webaoArtistReflect.Search("black", 1);

			return artists;
		}

		public static Object testDynArtistGetInfo()
		{
			return webaoArtistDyn.GetInfo("muse");
		}
		public static Object testDyndArtistSearch()
		{
			return webaoArtistDyn.Search("black", 1);
		}

		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testReflectArtistGetInfo), "Build Artist GetInfo", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testDynArtistGetInfo), "Emmit Artist GetInfo", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testReflectArtistSearch), " Build Artist Search", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(ArtistBench.testDyndArtistSearch), "Emmit Artist Search", ITER_TIME, NUM_WARMUP, NUM_ITER);

		}
	}
}
