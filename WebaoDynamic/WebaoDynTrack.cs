using System.Collections.Generic;
using Webao.Attributes;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    //[AddParameter("format", "json")]
    //[AddParameter("api_key", LastFmAPI.API_KEY)]
    public interface WebaoDynTrack
    {
        [Get("?method=geo.gettoptracks&country={country}")]
        [Mapping(typeof(DtoGeoTopTracks), ".Tracks.Track")]
        List<Track> GeoGetTopTracks(string country);

    }
}
