﻿using System.Collections.Generic;

namespace Webao.Dto
{
    public class DtoSearch
    {
        public DtoResults Results { get; set; }
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