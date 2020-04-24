﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using WebaoDynamic;
using WebaoDynDummy;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicTest
    {
        static readonly WebaoArtistDummy webaoArtistDummy = new WebaoArtistDummy(new HttpRequest());
        static readonly WebaoArtistDummy webaoArtistDummyMock = new WebaoArtistDummy(new LastfmMockRequest());

        static readonly WebaoBoredomDummy boredomWebao = (WebaoBoredomDummy)WebaoDynBuilder.Build(typeof(WebaoDynBoredom), new HttpRequest());
        static readonly WebaoBoredomDummy boredomWebaoMock = new WebaoBoredomDummy(new MockRequest());

        static readonly WebaoCountryDummy countryWebao = new WebaoCountryDummy(new HttpRequest());
        static readonly WebaoCountryDummy countryWebaoMock = new WebaoCountryDummy(new MockRequest());

        static readonly WebaoTrackDummy trackWebao = new WebaoTrackDummy(new HttpRequest());
        static readonly WebaoTrackDummy trackWebaoMock = new WebaoTrackDummy(new MockRequest());

        static readonly WebaoCharacterDummy webaoCharacterDummy = new WebaoCharacterDummy(new HttpRequest());
        static readonly WebaoCharacterDummy webaoCharacterDummyMock = new WebaoCharacterDummy(new MockRequest());

        [Test]
        public void TestWebaoArtist()
        {
            Artist artist = webaoArtistDummy.GetInfo("muse");
            Assert.AreEqual("Muse", artist.Name);
            Assert.AreEqual("fd857293-5ab8-40de-b29e-55a69d4e4d0f", artist.Mbid);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist.Url);
            Assert.AreNotEqual(0, artist.Stats.Listeners);
            Assert.AreNotEqual(0, artist.Stats.Playcount);
        }

        [Test]
        public void TestWebaoArtistMock()
        {
            Artist artist = webaoArtistDummyMock.GetInfo("muse");
            Assert.AreEqual("Muse", artist.Name);
            Assert.AreEqual("fd857293-5ab8-40de-b29e-55a69d4e4d0f", artist.Mbid);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist.Url);
            Assert.AreNotEqual(0, artist.Stats.Listeners);
            Assert.AreNotEqual(0, artist.Stats.Playcount);
        }

        [Test] 
        public void TestWebaoArtistSearch()
        {
            List<Artist> artists = webaoArtistDummy.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        [Test]
        public void TestWebaoArtistSearchMock()
        {
            List<Artist> artists = webaoArtistDummyMock.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
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
        public void TestWebaoBoredomParticipants()
        {
            Boredom boredom = boredomWebao.GetActivity(1, 0.6f);
            Assert.AreEqual("Learn the Chinese erhu", boredom.Activity);
            Assert.AreEqual("music", boredom.Type);
        }

        [Test]
        public void TestWebaoBoredomParticipantsMock()
        {
            Boredom boredom = boredomWebaoMock.GetActivity(1, 0.6f);
            Assert.AreEqual("Learn the Chinese erhu", boredom.Activity);
            Assert.AreEqual("music", boredom.Type);
        }
         
        [Test]
        public void TestWebaoDynCountry()
        {
            List<Country> country = countryWebao.GetNationality("luis");
            Assert.AreEqual("PE", country[0].Country_Id);
            Assert.AreEqual(0.06323779f, country[0].Probability);
        }

        [Test]
        public void TestWebaoDynCountryMock()
        {
            List<Country> country = countryWebaoMock.GetNationality("luis");
            Assert.AreEqual("PE", country[0].Country_Id);
            Assert.AreEqual(0.06323779f, country[0].Probability);
        }

        [Test]
        public void TestWebaoDynTrack()
        {
            List<Track> tracks = trackWebaoMock.GeoGetTopTracks("australia");
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }

        [Test]
        public void TestWebaoDynTrackMock()
        {
            List<Track> tracks = trackWebaoMock.GeoGetTopTracks("australia");
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }

        [Test]
        public void TestWebaoCharacter()
        {
            Character character = webaoCharacterDummy.GetCharacter(583);
            Assert.AreEqual("Jon Snow", character.Name);
            Assert.AreEqual("Northmen", character.Culture);
            Assert.AreEqual("In 283 AC", character.Born);
        }

        [Test]
        public void TestWebaoCharacterMock()
        {
            Character character = webaoCharacterDummyMock.GetCharacter(583);
            Assert.AreEqual("Jon Snow", character.Name);
            Assert.AreEqual("Northmen", character.Culture);
            Assert.AreEqual("In 283 AC", character.Born);
        }
    }
}
