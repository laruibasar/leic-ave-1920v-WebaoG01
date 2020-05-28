using System.Collections.Generic;

namespace Webao.Dto
{
    public struct DtoGeoTopTracks
    {
        public DtoTracks Tracks { get; set; }

        public List<Track> GetTracks()
        {
            return this.Tracks.Track;
        }
    }

    public struct DtoTracks
    {
        public List<Track> Track { get; set; }
    }

}