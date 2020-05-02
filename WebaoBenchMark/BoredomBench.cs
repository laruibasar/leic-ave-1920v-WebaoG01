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
    class BoredomBench
	{
		static readonly IWebaoBoredom webaoBoredomBuild = (IWebaoBoredom)WebaoDynBuilder.Build(typeof(IWebaoBoredom), new MockRequest());
		static readonly IWebaoBoredom webaoBoredomEmmit = (IWebaoBoredom)WebaoEmitter.ConstructorEmitter(typeof(IWebaoBoredom), new LastfmMockRequest());

		public static Object testBuildBoredomGetActivityByKey()
		{
			return webaoBoredomBuild.GetActivityByKey(5881028);
		}
		public static Object testBuildBoredomGetActivity()
		{
			return webaoBoredomBuild.GetActivity(1, 0.6f);
		}

		public static Object testEmmitBoredomGetActivityByKey()
		{
			return webaoBoredomEmmit.GetActivityByKey(5881028);
		}
		public static Object testEmmitBoredomGetActivity()
		{
			return webaoBoredomEmmit.GetActivity(1, 0.6f);
		}

		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testBuildBoredomGetActivityByKey), "testBuildBoredomGetActivityByKey", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testEmmitBoredomGetActivityByKey), "testEmmitBoredomGetActivityByKey", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testBuildBoredomGetActivity), "testBuildBoredomGetActivity", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testEmmitBoredomGetActivity), "testEmmitBoredomGetActivity", ITER_TIME, NUM_WARMUP, NUM_ITER);

		}
	}
}