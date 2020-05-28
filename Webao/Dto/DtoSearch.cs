using System.Collections.Generic;

namespace Webao.Dto
{
    public class DtoSearch
    {
        public DtoResults Results { get; set; }

        public List<Artist> GetArtistList()
        {
            return this.Results.ArtistMatches.Artist;
        }
    }

    public class DtoResults
    {
        public DtoArtistMatches ArtistMatches { get; set; }
    }

    public class DtoArtistMatches
    {
        public List<Artist> Artist { get; set; }
    }     

}