using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy  
{
    public class WebaoArtistDummy : WebaoDynArtist
    {
        private readonly IRequest req;
        public WebaoArtistDummy(IRequest req)
        {
            this.req = req;
            req.BaseUrl("http://ws.audioscrobbler.com/2.0/");
            req.AddParameter("format", "json");
            req.AddParameter("api_key", "a6c9a2229d0a79160dd93641841b0676");
        }

        public Artist GetInfo(string name)
        {
            string path = "?method=artist.getinfo&artist={name}";
            path = path.Replace("{name}", name.ToString());

            Type type = typeof(DtoArtist);
            DtoArtist dto = (DtoArtist)req.Get(path, type);

            return dto.Artist;
        } 

        public List<Artist> Search(string name, int page)
        {
            string path = "?method=artist.getinfo&artist={name}";
            path = path.Replace("{name}", name.ToString());
            path = path.Replace("{page}", page.ToString());

            Type type = typeof(DtoSearch);
            DtoSearch dto = (DtoSearch)req.Get(path, type);

            return dto.Results.ArtistMatches.Artist;
        }
    }
}
