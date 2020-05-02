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
		static readonly IWebaoCharacter webaoCharacterBuild = (IWebaoCharacter)WebaoDynBuilder.Build(typeof(IWebaoCharacter), new MockRequest());
		static readonly IWebaoCharacter webaoCharacterEmmit = (IWebaoCharacter)WebaoEmitter.ConstructorEmitter(typeof(IWebaoCharacter), new LastfmMockRequest());

		public static Object testBuildCharacterGetCharacter()
		{
			return webaoCharacterBuild.GetCharacter(583);
		}
		public static Object testEmmitCharacterGetCharacter()
		{
			return webaoCharacterEmmit.GetCharacter(583);
		}


		public static void Run()
		{
			const long ITER_TIME = 1000;
			const long NUM_WARMUP = 10;
			const long NUM_ITER = 10;

			//			NBench.Benchmark(new BenchmarkMethod(NBench.nullTest), "nullTest", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CharacterBench.testBuildCharacterGetCharacter), "testBuildCharacterGetCharacter", ITER_TIME, NUM_WARMUP, NUM_ITER);
			NBench.Benchmark(new BenchmarkMethod(CharacterBench.testEmmitCharacterGetCharacter), "testEmmitCharacterGetCharacter", ITER_TIME, NUM_WARMUP, NUM_ITER);
	
		}
	}
}