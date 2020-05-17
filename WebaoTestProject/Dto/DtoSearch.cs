using System.Collections.Generic;

namespace WebaoTestProject.Dto
{
    public class DtoSearch
    {
        public DtoResults Results { get; set; }

        public List<Artist> GetArtistsList()
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