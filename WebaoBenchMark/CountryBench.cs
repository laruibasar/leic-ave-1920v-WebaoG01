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
		static readonly IWebaoCountry webaoCountryDyn = (IWebaoCountry)WebaoDynBuilder.Build(typeof(IWebaoCountry), new MockRequest());
		static readonly WebaoCountry countryWebaoReflect = (WebaoCountry)WebaoBuilder.Build(typeof(WebaoCountry), new MockRequest());

		public static Object testDynCountryGetNationality()
		{
			return webaoCountryDyn.GetNationality("luis");
		}
		

		public static Object testReflectCountryGetNationality()
		{
			return countryWebaoReflect.GetNationality("luis");
		}
		

		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CountryBench.testReflectCountryGetNationality), "testReflectCountryGetNationality", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CountryBench.testDynCountryGetNationality), "testDynCountryGetNationality", ITER_TIME, NUM_WARMUP, NUM_ITER);


		}
	}
}