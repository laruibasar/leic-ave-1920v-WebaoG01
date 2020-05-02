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
		static readonly IWebaoTrack webaoTrackDyn = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new MockRequest());
		static readonly WebaoTrack webaoTrackReflect = (WebaoTrack)WebaoBuilder.Build(typeof(WebaoTrack), new LastfmMockRequest());


		public static Object testDynTrackGeoGetTopTracks()
		{
			return webaoTrackDyn.GeoGetTopTracks("australia");
		}


		public static Object testReflectTrackGeoGetTopTracks()
		{
			return webaoTrackReflect.GeoGetTopTracks("australia");
		}


		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(TrackBench.testReflectTrackGeoGetTopTracks), "testReflectTrackGeoGetTopTracks", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(TrackBench.testDynTrackGeoGetTopTracks), "testDynTrackGeoGetTopTracks", ITER_TIME, NUM_WARMUP, NUM_ITER);

		}
	}
}