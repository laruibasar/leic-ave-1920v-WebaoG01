using NUnit.Framework;
using System.Collections.Generic;
using Webao;
using WebaoDynamic.Part3;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    [TestFixture]
    public class WebaoDynamicLazyTest
    {
        static readonly ServiceArtists serviceArtists = new ServiceArtists(new HttpRequestLazy());
        static readonly ServiceCharacters serviceCharacters = new ServiceCharacters(new HttpRequestLazy());
        static readonly ServiceCountries serviceCountries = new ServiceCountries(new HttpRequestLazy());
        static readonly ServiceTracks serviceTracks = new ServiceTracks(new HttpRequestLazy());


        [Test]
        public void TestServiceTracks()
        {
            IEnumerable<Track> tracks = serviceTracks.TopTracksFrom("australia");
            int count = 0;
            foreach (Track track in tracks)
            {
                count++;
                if (count == 50)
                {
                    Assert.AreEqual(50, count);
                    break;
                }
            }
        }

        [Test]
        public void TestServiceCountries()
        {
            IEnumerable<Country> countries = serviceCountries.GetNationality("luis");
            int count = 0;
            foreach (Country track in countries)
            {
                count++;
                if (count == 50)
                {
                    Assert.AreEqual(50, count);
                    break;
                }
            }
        }


        [Test]
        public void TestServiceCharacters()
        {
            IEnumerable<Character> characters = serviceCharacters.GetList();
            int count = 0;
            foreach (Character track in characters)
            {
                count++;
                if (count == 50)
                {
                    Assert.AreEqual(50, count);
                    break;
                }
            }
        }


        [Test]
        public void TestServiceArtists()
        {
            IEnumerable<Artist> artists = serviceArtists.Search("black", 1);
            int count = 0;
            foreach (Artist track in artists)
            {
                count++;
                if (count == 50)
                {
                    Assert.AreEqual(50, count);
                    break;
                }
            }
        }

    }
}
