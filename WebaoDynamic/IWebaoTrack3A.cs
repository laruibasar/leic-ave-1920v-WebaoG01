using System.Collections.Generic;
using Webao.Attributes;
using Webao.Base;
using WebaoTestProject.Dto;

namespace WebaoDynamic
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameter("format", "json")]
    [AddParameter("api_key", LastFmAPI.API_KEY)]
    public interface IWebaoTrack3A
    {
        [Get("?method=geo.gettoptracks&country={country}")]
        [Mapping(typeof(DtoGeoTopTracks), With = "WebaoTestProject.Dto.DtoGeoTopTracks.GetGeoTopTracks")]
        List<Track> GeoGetTopTracks(string country);
    }
}
