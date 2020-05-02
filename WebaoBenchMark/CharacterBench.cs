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
    class CharacterBench
	{
		static readonly IWebaoCharacter webaoCharacterDyn = (IWebaoCharacter)WebaoDynBuilder.Build(typeof(IWebaoCharacter), new MockRequest());
		static readonly WebaoCharacter characterWebaoReflect = (WebaoCharacter)WebaoBuilder.Build(typeof(WebaoCharacter), new MockRequest());

		public static Object testDynCharacterGetCharacter()
		{
			return webaoCharacterDyn.GetCharacter(583);
		}
		public static Object testReflectCharacterGetCharacter()
		{
			return characterWebaoReflect.GetCharacter(583);
		}


		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CharacterBench.testReflectCharacterGetCharacter), "testReflectCharacterGetCharacter", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CharacterBench.testDynCharacterGetCharacter), "testDynCharacterGetCharacter", ITER_TIME, NUM_WARMUP, NUM_ITER);
	
		}
	}
}