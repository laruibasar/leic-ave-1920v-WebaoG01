using System;
using System.Collections.Generic;
using Webao;
using Webao.Dto;
using WebaoDynamics;
using WebaoDynamics.Interfaces;
using WebaoTestProject;

namespace WebaoBench
{
    public class WebaoBench
    {
        static readonly WebaoArtist artistWebaoMock = (WebaoArtist)WebaoBuilder.Build(typeof(WebaoArtist), new LastfmMockRequest());
        static readonly WebaoTrack trackWebaoMock = (WebaoTrack)WebaoBuilder.Build(typeof(WebaoTrack), new LastfmMockRequest());
        static readonly WebaoBoredom boredomWebaoMock = (WebaoBoredom)WebaoBuilder.Build(typeof(WebaoBoredom), new MockRequest());
        static readonly WebaoCountry countryWebaoMock = (WebaoCountry)WebaoBuilder.Build(typeof(WebaoCountry), new MockRequest());
        static readonly WebaoCharacter characterWebaoMock = (WebaoCharacter)WebaoBuilder.Build(typeof(WebaoCharacter), new MockRequest());

        static readonly IWebaoArtist artistWebao = (IWebaoArtist)WebaoDynBuilder.Build(typeof(IWebaoArtist), new LastfmMockRequest());
        static readonly IWebaoBoredom boredomWebao = (IWebaoBoredom)WebaoDynBuilder.Build(typeof(IWebaoBoredom), new MockRequest());
        static readonly IWebaoCountry countryWebao = (IWebaoCountry)WebaoDynBuilder.Build(typeof(IWebaoCountry), new MockRequest());
        static readonly IWebaoTrack trackWebao = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new MockRequest());
        static readonly IWebaoCharacter characterWebao = (IWebaoCharacter)WebaoDynBuilder.Build(typeof(IWebaoCharacter), new MockRequest());

        public static Artist callArtistReflect()
        {
            return artistWebaoMock.GetInfo("muse");
        }

        public static Artist callArtistEmitter()
        {
            return artistWebao.GetInfo("muse");
        }

        public static List<Track> callTrackReflect()
        {
            return trackWebaoMock.GeoGetTopTracks("australia");
        }

        public static List<Track> callTrackEmitter()
        {
            return trackWebao.GeoGetTopTracks("australia");
        }

        public static Boredom callBoredomReflect()
        {
            return boredomWebaoMock.GetActivityByKey(5881028);
        }

        public static Boredom callBoredomEmitter()
        {
            return boredomWebao.GetActivityByKey(5881028);
        }

        public static List<Country> callCountryReflect()
        {
            return countryWebaoMock.GetNationality("luis");
        }

        public static List<Country> callCountryEmitter()
        {
            return countryWebao.GetNationality("luis");
        }

        public static Character callCharacterReflect()
        {
            return characterWebaoMock.GetCharacter(583);
        }

        public static Character callCharacterEmitter()
        {
            return characterWebao.GetCharacter(583);
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Benchmark LI41N-G01!");

            const long ITER_TIME = 1000;
            const long NUM_WARMUP = 10;
            const long NUM_ITER = 10;

            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callArtistReflect), "Teste Artist Refleção", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callArtistEmitter), "Teste Artist IL Emitter", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callTrackReflect), "Teste Track Refleção", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callTrackEmitter), "Teste Track IL Emitter", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callBoredomReflect), "Teste Boredom Refleção", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callBoredomEmitter), "Teste Boredom IL Emitter", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callCountryReflect), "Teste Country Refleção", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callCountryEmitter), "Teste Country IL Emitter", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callCharacterReflect), "Teste Character Refleção", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(WebaoBench.callCharacterEmitter), "Teste Character IL Emitter", ITER_TIME, NUM_WARMUP, NUM_ITER);
        }
    }
}
