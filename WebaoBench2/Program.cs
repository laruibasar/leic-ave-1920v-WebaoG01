using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoBench2
{
    class MainClass
    {
        public static WebaoArtist artistWebaoMock = (WebaoArtist)WebaoBuilder.Build(typeof(WebaoArtist), new LastfmMockRequest());
        public static IWebaoArtist webaoArtistMock = (IWebaoArtist)WebaoDynamic.WebaoDynBuilder.Build(typeof(IWebaoArtist), new LastfmMockRequest());

        public static WebaoTrack trackWebaoMock = (WebaoTrack)WebaoBuilder.Build(typeof(WebaoTrack), new LastfmMockRequest());
        public static IWebaoTrack webaoTrackMock = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new MockRequest());

        static readonly WebaoBoredom boredomWebaoMock = (WebaoBoredom)WebaoBuilder.Build(typeof(WebaoBoredom), new MockRequest());
        static readonly IWebaoBoredom webaoBoredomMock = (IWebaoBoredom)WebaoDynBuilder.Build(typeof(IWebaoBoredom), new MockRequest());

        static readonly WebaoCountry countryWebaoMock = (WebaoCountry)WebaoBuilder.Build(typeof(WebaoCountry), new MockRequest());
        static readonly IWebaoCountry webaoCountryMock = (IWebaoCountry)WebaoDynBuilder.Build(typeof(IWebaoCountry), new MockRequest());

        static readonly WebaoCharacter characterWebaoMock = (WebaoCharacter)WebaoBuilder.Build(typeof(WebaoCharacter), new MockRequest());
        static readonly IWebaoCharacter webaoCharacterDummyMock = (IWebaoCharacter)WebaoDynBuilder.Build(typeof(IWebaoCharacter), new MockRequest());

        public static Object call1a() 
        {           
            Artist artist = artistWebaoMock.GetInfo("muse");
            return artist; 
        }

        public static Object call1b()
        {            
            Artist artist = webaoArtistMock.GetInfo("muse");
            return artist;
        }

        public static Object call2a()
        {            
            List<Track> tracks = trackWebaoMock.GeoGetTopTracks("australia");
            return tracks;
        }

        public static Object call2b()
        {            
            List<Track> tracks = webaoTrackMock.GeoGetTopTracks("australia");
            return tracks;
        }

        public static Object call3a()
        {
            Boredom boredom = boredomWebaoMock.GetActivityByKey(5881028);
            return boredom;
        }

        public static Object call3b()
        {
            Boredom boredom = webaoBoredomMock.GetActivityByKey(5881028);
            return boredom;
        }

        public static Object call4a()
        {
            List<Country> country = countryWebaoMock.GetNationality("luis");
            return country;
        }

        public static Object call4b()
        {
            List<Country> country = webaoCountryMock.GetNationality("luis");
            return country;
        }

        public static Object call5a()
        {
            Character character = characterWebaoMock.GetCharacter(583);
            return character;
        }

        public static Object call5b()
        {
            Character character = webaoCharacterDummyMock.GetCharacter(583);
            return character;
        }

        static void Main(string[] args)  
        {
            const long ITER_TIME = 1000;
            const long NUM_WARMUP = 10;
            const long NUM_ITER = 10;

            Console.WriteLine("START!");  
            NBench.Benchmark(new BenchmarkMethod(call1a), "WebaoArtist", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(call1b), "WebaoArtist Dyn", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(call2a), "WebaoTrack", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(call2b), "WebaoTrack Dyn", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(call3a), "WebaoBoredom", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(call3b), "WebaoBoredom Dyn", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(call4a), "WebaoCountry", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(call4b), "WebaoCountry Dyn", ITER_TIME, NUM_WARMUP, NUM_ITER);

            NBench.Benchmark(new BenchmarkMethod(call5a), "WebaoCharacter", ITER_TIME, NUM_WARMUP, NUM_ITER);
            NBench.Benchmark(new BenchmarkMethod(call5b), "WebaoCharacter Dyn", ITER_TIME, NUM_WARMUP, NUM_ITER);
        }
    }
}
