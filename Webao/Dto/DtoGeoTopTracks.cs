using System.Collections.Generic;

namespace Webao.Dto
{
    public struct DtoGeoTopTracks
    {
        public DtoTracks Tracks { get; set; }
    }

    public struct DtoTracks
    {
        public List<Track> Track { get; set; }
    }

}