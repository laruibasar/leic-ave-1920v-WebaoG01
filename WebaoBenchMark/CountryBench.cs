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
    class CountryBench
	{
		static readonly IWebaoCountry webaoCountryBuild = (IWebaoCountry)WebaoDynBuilder.Build(typeof(IWebaoCountry), new MockRequest());
		static readonly IWebaoCountry webaoCountryEmmit = (IWebaoCountry)WebaoEmitter.ConstructorEmitter(typeof(IWebaoCountry), new LastfmMockRequest());

		public static Object testBuildCountryGetNationality()
		{
			return webaoCountryBuild.GetNationality("luis");
		}
		

		public static Object testEmmitCountryGetNationality()
		{
			return webaoCountryEmmit.GetNationality("luis");
		}
		

		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CountryBench.testBuildCountryGetNationality), "testBuildCountryGetNationality", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CountryBench.testEmmitCountryGetNationality), "testEmmitCountryGetNationality", ITER_TIME, NUM_WARMUP, NUM_ITER);


		}
	}
}