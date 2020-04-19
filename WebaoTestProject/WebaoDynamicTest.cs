using System;
using System.Collections.Generic;
using NUnit.Framework;
using Webao;
using WebaoDynDummy;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicTest
    {
        static readonly WebaoBoredomDummy boredomWebao = new WebaoBoredomDummy(new HttpRequest());
        static readonly WebaoBoredomDummy boredomWebaoMock = new WebaoBoredomDummy(new MockRequest());

        static readonly WebaoCountryDummy countryWebao = new WebaoCountryDummy(new HttpRequest());
        static readonly WebaoCountryDummy countryWebaoMock = new WebaoCountryDummy(new MockRequest());

        static readonly WebaoTrackDummy trackWebao = new WebaoTrackDummy(new HttpRequest());
        static readonly WebaoTrackDummy trackWebaoMock = new WebaoTrackDummy(new MockRequest());

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
    }
}
