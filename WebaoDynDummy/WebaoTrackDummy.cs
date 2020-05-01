using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    public class WebaoTrackDummy : WebaoDyn, IWebaoTrack
    {
        public WebaoTrackDummy(IRequest req) : base(req)
        {
            base.SetUrl("http://ws.audioscrobbler.com/2.0/");
            base.SetParameter("format", "json");
            base.SetParameter("api_key", "a6c9a2229d0a79160dd93641841b0676");
        }

        public List<Track> GeoGetTopTracks(string country)
        {
            string path = "?method=geo.gettoptracks&country={country}";
            path = path.Replace("{country}", country);

            DtoGeoTopTracks dto = (DtoGeoTopTracks)base.GetRequest(path, typeof(DtoGeoTopTracks));

            return dto.Tracks.Track;
        }
    }
}
