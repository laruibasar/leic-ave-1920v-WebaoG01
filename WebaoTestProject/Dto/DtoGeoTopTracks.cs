using System.Collections.Generic;

namespace WebaoTestProject.Dto
{
    public struct DtoGeoTopTracks
    {
        public DtoTracks Tracks { get; set; }

        public List<Track> GetGeoTopTracks()
        { return this.Tracks.Track; }
    }

    public struct DtoTracks
    {
        public List<Track> Track { get; set; }
    }

}