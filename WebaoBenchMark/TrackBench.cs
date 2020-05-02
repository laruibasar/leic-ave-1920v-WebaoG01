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
    class TrackBench
	{
		static readonly IWebaoTrack webaoTrackBuild = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new MockRequest());
		static readonly IWebaoTrack webaoTrackEmmit = (IWebaoTrack)WebaoEmitter.ConstructorEmitter(typeof(IWebaoTrack), new LastfmMockRequest());


		public static Object testBuildTrackGeoGetTopTracks()
		{
			return webaoTrackBuild.GeoGetTopTracks("australia");
		}


		public static Object testEmmitTrackGeoGetTopTracks()
		{
			return webaoTrackEmmit.GeoGetTopTracks("australia");
		}


		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(TrackBench.testBuildTrackGeoGetTopTracks), "testBuildTrackGeoGetTopTracks", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(TrackBench.testEmmitTrackGeoGetTopTracks), "testEmmitTrackGeoGetTopTracks", ITER_TIME, NUM_WARMUP, NUM_ITER);

		}
	}
}