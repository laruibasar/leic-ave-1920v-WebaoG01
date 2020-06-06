using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicTest3A
    {
        static readonly IWebaoArtist3A webaoArtist = (IWebaoArtist3A)WebaoDynBuilder3.Build(typeof(IWebaoArtist3A), new HttpRequest());
        static readonly IWebaoArtist3A webaoArtistMock = (IWebaoArtist3A)WebaoDynBuilder3.Build(typeof(IWebaoArtist3A), new HttpRequest());

        static readonly IWebaoTrack trackWebao = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new HttpRequest());
        static readonly IWebaoTrack trackWebaoMock = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new MockRequest());

        [Test] 
        public void TestWebaoArtistSearch()
        {
            List<Artist> artists = webaoArtist.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        [Test]
        public void TestWebaoArtistMockSearch()
        {
            List<Artist> artists = webaoArtistMock.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        [Test]
        public void TestWebaoTrack()
        {
            List<Track> tracks = trackWebao.GeoGetTopTracks("australia");
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }

        [Test]
        public void TestWebaoTrackMock()
        {
            List<Track> tracks = trackWebaoMock.GeoGetTopTracks("australia");
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }
    }
}