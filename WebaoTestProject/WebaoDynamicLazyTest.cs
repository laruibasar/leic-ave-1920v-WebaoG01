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
                    Assert.Equals(50, count);
                    break;
                }
            }
        }
    }
}
