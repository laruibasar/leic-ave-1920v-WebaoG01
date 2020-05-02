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
		static readonly IWebaoBoredom webaoBoredomDyn = (IWebaoBoredom)WebaoDynBuilder.Build(typeof(IWebaoBoredom), new MockRequest());
		static readonly WebaoBoredom boredomWebaoReflect = (WebaoBoredom)WebaoBuilder.Build(typeof(WebaoBoredom), new MockRequest());

		public static Object testDynBoredomGetActivityByKey()
		{
			return webaoBoredomDyn.GetActivityByKey(5881028);
		}
		public static Object testDynBoredomGetActivity()
		{
			return webaoBoredomDyn.GetActivity(1, 0.6f);
		}

		public static Object testReflectBoredomGetActivityByKey()
		{
			return boredomWebaoReflect.GetActivityByKey(5881028);
		}
		public static Object testReflectBoredomGetActivity()
		{
			return boredomWebaoReflect.GetActivity(1, 0.6f);
		}

		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testReflectBoredomGetActivityByKey), "testReflectBoredomGetActivityByKey", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testDynBoredomGetActivityByKey), "testDynBoredomGetActivityByKey", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testReflectBoredomGetActivity), "testReflectBoredomGetActivity", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(BoredomBench.testDynBoredomGetActivity), "testDynBoredomGetActivity", ITER_TIME, NUM_WARMUP, NUM_ITER);

		}
	}
}