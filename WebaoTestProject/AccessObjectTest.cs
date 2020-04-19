using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class AccessObjectTest 
    {
        static readonly WebaoArtist artistWebao = (WebaoArtist) WebaoBuilder.Build(typeof(WebaoArtist), new HttpRequest());
        static readonly WebaoArtist artistWebaoMock = (WebaoArtist) WebaoBuilder.Build(typeof(WebaoArtist), new LastfmMockRequest());

        static readonly WebaoTrack trackWebao = (WebaoTrack) WebaoBuilder.Build(typeof(WebaoTrack), new HttpRequest());
        static readonly WebaoTrack trackWebaoMock = (WebaoTrack) WebaoBuilder.Build(typeof(WebaoTrack), new LastfmMockRequest());

        static readonly WebaoBoredom boredomWebao = (WebaoBoredom)WebaoBuilder.Build(typeof(WebaoBoredom), new HttpRequest());
        static readonly WebaoBoredom boredomWebaoMock = (WebaoBoredom)WebaoBuilder.Build(typeof(WebaoBoredom), new MockRequest());

        static readonly WebaoCountry countryWebao = (WebaoCountry)WebaoBuilder.Build(typeof(WebaoCountry), new HttpRequest());
        static readonly WebaoCountry countryWebaoMock = (WebaoCountry)WebaoBuilder.Build(typeof(WebaoCountry), new MockRequest());

        static readonly WebaoCharacter characterWebao = (WebaoCharacter)WebaoBuilder.Build(typeof(WebaoCharacter), new HttpRequest());
        static readonly WebaoCharacter characterWebaoMock = (WebaoCharacter)WebaoBuilder.Build(typeof(WebaoCharacter), new MockRequest());

        [Test]
        public void TestWebaoArtistGetInfo()  
        {
            Artist artist = artistWebao.GetInfo("muse");
            Assert.AreEqual("Muse", artist.Name);
            Assert.AreEqual("fd857293-5ab8-40de-b29e-55a69d4e4d0f", artist.Mbid);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist.Url);
            Assert.AreNotEqual(0, artist.Stats.Listeners);
            Assert.AreNotEqual(0, artist.Stats.Playcount);
        }

        [Test]
        public void TestWebaoArtistGetInfoMock()
        {
            Artist artist = artistWebaoMock.GetInfo("muse");
            Assert.AreEqual("Muse", artist.Name);
            Assert.AreEqual("fd857293-5ab8-40de-b29e-55a69d4e4d0f", artist.Mbid);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist.Url);
            Assert.AreNotEqual(0, artist.Stats.Listeners);
            Assert.AreNotEqual(0, artist.Stats.Playcount);
        }

        [Test]
        public void TestWebaoArtistSearch()
        {
            List<Artist> artists = artistWebao.Search("black");
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        [Test]
        public void TestWebaoArtistSearch2()
        {
            List<Artist> artists = artistWebao.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        [Test]
        public void TestWebaoArtistSearchMock()
        {
            List<Artist> artists = artistWebaoMock.Search("black");
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        [Test]
        public void TestWebaoTrackGeoGetTopTracks()
        {
            List<Track> tracks = trackWebao.GeoGetTopTracks("australia");
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }

        [Test]
        public void TestWebaoTrackGeoGetTopTracksMock()
        {
            List<Track> tracks = trackWebaoMock.GeoGetTopTracks("australia");
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }

        [Test]
        public void TestWebaoBoredom()
        {
            Boredom boredom = boredomWebao.GetActivityByKey(5881028);
            Assert.AreEqual("Learn a new programming language", boredom.Activity);
            Assert.AreEqual("education", boredom.Type);
            Assert.AreEqual(1, boredom.Participants);
        }

        [Test]
        public void TestWebaoBoredomMock()
        {
            Boredom boredom = boredomWebaoMock.GetActivityByKey(5881028);
            Assert.AreEqual("Learn a new programming language", boredom.Activity);
            Assert.AreEqual("education", boredom.Type);
            Assert.AreEqual(1, boredom.Participants);
        }

        [Test]
        public void TestWebaoBoredomParticipantsMock()
        {
            Boredom boredom = boredomWebao.GetActivity(1, 0.6f);
            Assert.AreEqual("Learn the Chinese erhu", boredom.Activity);
            Assert.AreEqual("music", boredom.Type);
        }

        [Test]
        public void TestWebaoCountry()
        {
            List<Country> country = countryWebao.GetNationality("luis");
            Assert.AreEqual("PE", country[0].Country_Id);
            Assert.AreEqual(0.06323779f, country[0].Probability);
        }

        [Test]
        public void TestWebaoCountryMock()
        {
            List<Country> country = countryWebaoMock.GetNationality("luis");
            Assert.AreEqual("PE", country[0].Country_Id);
            Assert.AreEqual(0.06323779f, country[0].Probability);
        }

        [Test]
        public void TestWebaoCharacter()
        {
            Character character = characterWebao.GetCharacter(583);
            Assert.AreEqual("Jon Snow", character.Name);
            Assert.AreEqual("Northmen", character.Culture);
            Assert.AreEqual("In 283 AC", character.Born); 
        }

        [Test]
        public void TestWebaoCharacterMock()
        {
            Character character = characterWebaoMock.GetCharacter(583);
            Assert.AreEqual("Jon Snow", character.Name);
            Assert.AreEqual("Northmen", character.Culture);
            Assert.AreEqual("In 283 AC", character.Born);
        }
    }
}
