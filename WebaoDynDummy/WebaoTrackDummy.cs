using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Webao;
using Webao.Attributes;
using WebaoDynamic;
using WebaoTestProject.Dto;
using Webao.Base;

namespace WebaoDynDummy
{
    public class WebaoTrackDummy : WebaoDynTrack
    {
        private readonly IRequest req; 

        public WebaoTrackDummy(IRequest req)
        {
            this.req = req;
            req.BaseUrl("http://ws.audioscrobbler.com/2.0/");
            req.AddParameter("format", "json");
            req.AddParameter("api_key", "a6c9a2229d0a79160dd93641841b0676");
        }

        public List<Track> GeoGetTopTracks(string country)
        {
            string path = "?method=geo.gettoptracks&country={country}";
            path = path.Replace("{country}", country);

            Type type = typeof(DtoGeoTopTracks);
            DtoGeoTopTracks dto = (DtoGeoTopTracks)req.Get(path, type);

            return dto.Tracks.Track;
        }
    }
}
