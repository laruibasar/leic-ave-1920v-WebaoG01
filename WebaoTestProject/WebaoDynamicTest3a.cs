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
        /* Use the same WebaoDynBuilder because diference
         * is made on the WebaoEmitter class
         * allowing for dealing with With word
         */
        static readonly IWebaoArtist3A webaoArtist = (IWebaoArtist3A)WebaoDynBuilder.Build(typeof(IWebaoArtist3A), new HttpRequest());
        static readonly IWebaoArtist3A webaoArtistMock = (IWebaoArtist3A)WebaoDynBuilder.Build(typeof(IWebaoArtist3A), new HttpRequest());

        static readonly IWebaoTrack trackWebao = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new HttpRequest());
        static readonly IWebaoTrack trackWebaoMock = (IWebaoTrack)WebaoDynBuilder.Build(typeof(IWebaoTrack), new MockRequest());

        static readonly IWebaoCharacter characterWebao = (IWebaoCharacter)WebaoDynBuilder.Build(typeof(IWebaoCharacter), new HttpRequest());
        static readonly IWebaoCharacter characterWebaoMock = (IWebaoCharacter)WebaoDynBuilder.Build(typeof(IWebaoCharacter), new MockRequest());

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